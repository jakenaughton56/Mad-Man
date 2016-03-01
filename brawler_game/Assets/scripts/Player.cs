using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

// controls most of the player's movement
[RequireComponent(typeof(Controller2D))]
public class Player : NetworkBehaviour {

	public Animator animator;
	// editable player attributes (use inspection window)
	public float jumpHeight;
	public float timeToJump;
	public float moveSpeed;
	public float accelerationTimeAirbourne;
	public float accelerationTimeGrounded;
	private GameObject weapon;
	public GameObject joystick;
	private joystickController jc;
	public Vector2 input;
	// direction of the player
	private int dir;
	// have they fired in the last update
	public int hasFired;
	// player's health
	private int health;
	// player's last position
	public Vector3 lastPos;
	// player's healthbar
	public GameObject HealthBar;
	// direction in previous update
	private int prevDir;
	// are they alive
	public bool isDead;
	private float speedCooldown;

	// animation flags
	public const int STATE_IDLE = 0;
	public const int STATE_MELEE = 1;

	// the starting weapon
	public GameObject Melee;
	
	// the screen to popup at the end of the game
	public GameObject endGameScreen;

	// for controlling the healthbar
	private Vector3 healthBarPos;
	private Vector3 healthBarSize;

	// how much health the player starts with
	public int STARTHEALTH = 100;

	// game ended flag
	private bool gameFinished;

	int _currentAnimationState = STATE_IDLE;

	// for moving the joystick and setting it to the player's set location
	Transform js;
	Transform attackButton;
	Transform jumpButton;

	// the players that are in the game
	GameObject[] players;

	// to check whether all the players that were in the lobby have
	// loaded the level
	private bool allPlayersLoaded;

	// to store game stats
	private int kills;
	private int deaths;

	// to upload stats
	public GameObject databaseHandler;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float  velocitySmoothing;

	// controller which manages player movement
	Controller2D controller;

	// the last enemy player to shoot this player
	public GameObject lastHitBy;

	// check whether the number of players in the scene is equal to the number of players
	// that were in the lobby
	private bool getAllPlayers() {
		// the number of players that were in the lobby
		int numPlayers = playerCount.findCounter ().GetComponent<playerCount> ().getNumPlayers ();
		// find all players in the current scene
		players = GameObject.FindGameObjectsWithTag ("Player");
		// not all players have loaded, so need to try again next frame
		if (players.Length != numPlayers) {
			return false;
		} return true;

	}

	// projectile calls this when it hits the player
	public void takeDamageFromProjectile(GameObject projectile) {
		// this is the most recent projectile to hit the player
		lastHitBy = projectile.GetComponent<Projectile>().shooter;
		// get the damage of the projectile
		int projDmg = projectile.GetComponent<Projectile> ().getDamage ();
		// each player stores their own health then syncs it with other players
		// so only change health if this is the client
		if (isLocalPlayer) {
			changeHealth (-projDmg);
		}
	}
	
	void Start () {
		// set flags
		animator = this.GetComponent<Animator>();
		allPlayersLoaded = false;
		gameFinished = false;
		// give player a starting weapon
		weapon = Instantiate(Melee);

		// set dead bool
		isDead = false;
		// find all players in the game
		players = GameObject.FindGameObjectsWithTag ("Player");

		healthBarSize = HealthBar.transform.localScale;
		healthBarPos = HealthBar.transform.localPosition;

		lastPos = transform.position;
		health = STARTHEALTH;
		hasFired = 0;
		input = new Vector2 (0, 0);
		dir = -1;
		prevDir = -1;
		speedCooldown = 0;


		// set player position and create joystick for that player
		// only if they are the client
		if (isLocalPlayer) {
			// add a bit of randomness to their spawn position
			transform.position = new Vector3(Random.Range(-20, 20), 5, 0);
			// create a new joystick
			joystick = Instantiate (joystick);
			// get the joystick controller
			jc = joystick.GetComponentInChildren<joystickController> ();
		}

		// get the player controller
		controller = GetComponent<Controller2D> ();

		// calculate gravity and jump velocity using basic physics
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJump, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJump;

		// set counters
		kills = 0;
		deaths = 0;
	}


	// get keyboard input and update player's position
	void getInput() {
		
		if (controller.collisions.below) {
			velocity.y = 0;
		}
		
		// sum input from keyboard and joystick to get total movement
		// realistically only one control scheme will be used at a time
		input = new Vector2 (jc.getDirX() + Input.GetAxisRaw("Horizontal"), jc.getDirY() + Input.GetAxisRaw ("Vertical"));
		
		
		// check if player is trying to jump
		if ((jc.isJumping || Input.GetKey("space")) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}
		
		// calculate players movement speed
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocitySmoothing, 
		                               (controller.collisions.below)?
		                               accelerationTimeGrounded : accelerationTimeAirbourne);
		
		// add gravity to horizontal speed
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime, input.y < 0); // move player object

		// check for attack button press (either keyboard or on-screen)
		if (Input.GetKey ("z") || jc.isAttacking) {
			hasFired = 1;
			attack ();
		} else {
			if (isLocalPlayer) {
				hasFired = 0;
			}
		}
		// set player's direction according to their input
		if (input.x > 0.1f) {
			dir = 1;
		}
		else if (input.x < -0.1f) {
			dir = -1;
		}
		
	}

	// check if there is only one player alive
	// if yes, show end game screen
	void checkForEndGame() {
		int numDead = 0;
		// don't check if some players haven't loaded yet
		if (!allPlayersLoaded) {
			return;
		}
		// iterate through all players, checking if they are dead
		for (int i = 0; i < players.Length; i++) {
			GameObject p = players[i];
			if (p.GetComponent<Player>().isDead) {
				numDead++;
			}
		}
		// if all but one player is dead, the game is finished
		// make sure numDead is not zero, in this case the game will
		// only have 1 player in it so should never end
		if ((numDead == players.Length - 1 && numDead != 0) || players.Length == 0) {
			PlayerPrefs.SetInt("numKills", kills);
			// now update the stats to show on the end game menu
			if (this.isDead) {
				//e.setOutcome("You lost!");
				PlayerPrefs.SetInt("won", 0);
			} else {
				//e.setOutcome("You won!");
				PlayerPrefs.SetInt("won", 1);
			}
			if (isDead) {
				deaths = 1;
			} else {
				deaths = 0;
			}
			PlayerPrefs.SetInt("numDeaths", deaths);

			// if won, isDead will be false
			uploadEndGameStats(!isDead);
	
			// set bool
			gameFinished = true;
			// load end game screen
			Application.LoadLevel(7);

		}
	}



	// send the stats for this game to the database
	private void uploadEndGameStats(bool wonGame) {
		int won;
		if (wonGame) {
			won = 1;
		} else {
			won = 0;
		}
		// create new database handler and send stats through it
		GameObject db = Instantiate (databaseHandler);
		db.GetComponent<DBUtils>().upStats (PlayerPrefs.GetString("Name"), won, kills, 0);
	}

	void Update(){
		// no need to run update if the game is finished
		if (gameFinished) {
			return;
		}

		playAnim();

		// if not all players have loaded, check again
		if (!allPlayersLoaded) {
			allPlayersLoaded = getAllPlayers();
		}

		// only get player's input if they are the local player
		// otherwise each player will have control over all others
		if (isLocalPlayer) {
			getInput ();
			checkForEndGame();
			// check if they have died
			checkHealth ();
			// for using the boots
			if(speedCooldown > 0){
				speedCooldown -= (float)Time.deltaTime;
			}
			else if(speedCooldown <= 0){
				normalSpeed(20);
			}

		}

		// flip player's sprite if they have changed direction
		flip ();



		// update their health bar
		redrawHealthBar ();

		if (weapon != null) {
			// set weapon's direction
			weapon.GetComponent<Weapon> ().changeDir (dir);
		} else {
			// if player doesn't have a weapon, give them a melee weapon
			GameObject newMelee = Instantiate(Melee);
			weapon = newMelee;
			// set parent to be player
			newMelee.transform.parent = this.transform;
		}
	}

	// flip player's sprite
	private void flip() {
		// make sure player is facing the right way
		if (dir != prevDir) {
			transform.localScale = new Vector3 (-transform.localScale.x,
			                                    transform.localScale.y,
			                                    transform.localScale.z);
		}
		
		// update player's weapon so it looks like they are holding it
		if (weapon != null) {
			weapon.transform.position = new Vector3(transform.position.x, transform.position.y, -3);;
			if (false) {
				weapon.transform.localScale = new Vector3 (-weapon.transform.localScale.x,
				                                           weapon.transform.localScale.y,
				                                           weapon.transform.localScale.z);
			}
		}
		prevDir = dir;
	}

	// check the players health
	// if < 0, kill them
	private void checkHealth() {
		if (health < 0) {
			isDead = true;
			// move player off-screen
			// TODO - find better way to hide player
			transform.position = new Vector3(-100,-100,-100);
		}
	}

	// get player's direction
	public int getDir() {
		return dir;
	}
		
	// set player's health
	public void setHealth(int health) {
		this.health = health;
	}

	// called when we collide with an object
	void OnCollisionEnter2D(Collision2D coll)
	{

		// if we collide with a weapon, pick it up
		if (coll.gameObject.tag == "Weapon") {
			Destroy(this.weapon);
			weapon = coll.gameObject;
			// disable the collider for the weapon
			weapon.GetComponent<Collider2D>().enabled = false;
			// freeze the rigidbody on the weapon
			weapon.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			// fix the weapon's rotation
			weapon.transform.localScale = new Vector3 (-dir * weapon.transform.localScale.x,
			                                           weapon.transform.localScale.y,
			                                           weapon.transform.localScale.z);
			// set the weapon to be the player's child
			weapon.transform.parent = this.transform;
			weapon.GetComponent<Weapon>().cooldown = 0.5f;
		}
		// if we collide with a pickup, activate that pickup
		if (coll.gameObject.tag == "Pickup") {
			GameObject pickup = coll.gameObject;
			// call pick up method on the pickup item, send the player's script
			pickup.GetComponent<Pickup>().onPickUp(GetComponent<Player>());
			// destroy the pickup so it isnt used again
			Destroy (coll.gameObject);
		}
	}

	// attack
	void attack() {
		// don't attack if we are not the local player
		if (!isLocalPlayer) {
			return;
		}
		// melee
		if (weapon == null) {
		}
		// otherwise if player has a weapon, attack using weapon
		else if (weapon.GetComponent<Weapon> ().id == 1) {
			// run anim
			animator.SetInteger("State", STATE_MELEE);
			weapon.GetComponent<Weapon> ().attack ();
		}
		else {
			weapon.GetComponent<Weapon> ().attack ();
		}
	}

	// return the player's current weapon
	public GameObject getWeapon() {
		return weapon;
	}

	// add an amount to the player's health
	public void changeHealth(int amt) {
		health += amt;

	}

	// increase the player's speed
	public void increaseSpeed(int speed, float cooldown){
		if (isLocalPlayer) {
			moveSpeed = speed;
			speedCooldown = cooldown;
		}
	}

	// reset the player's speed
	public void normalSpeed(int speed){
		moveSpeed = 20;
	}

	// redraw the player's health bar
	public void redrawHealthBar() {
		float percentHealth = (float)health / STARTHEALTH;
		// update the health bar transform so its size and offset represents
		// the player's percentage health
		HealthBar.transform.localScale = new Vector3 (healthBarSize.x * percentHealth,
		                                              healthBarSize.y,
		                                              healthBarSize.z);
		Vector3 HBSize = HealthBar.transform.localScale;
		HealthBar.transform.localPosition = new Vector3 (healthBarPos.x - (healthBarSize.x - HBSize.x) / 2,
		                                                 healthBarPos.y,
		                                                 healthBarPos.z);
	}

	// return player's health
	public int getHealth() {
		return health;
	}

	// update player's direction
	public void setDir(int dir) {
		this.dir = dir;
	}

	// add a kill to the player's kill counter
	public void addKill() {
		kills++;
	}

	// play the player's punch animation
	public void playAnim() {
		if (weapon == null) {
			return;
		} else if (weapon.GetComponent<Weapon> ().cooldown <= 0) {
			animator.SetInteger ("State", STATE_IDLE);
		}
	}	

}
