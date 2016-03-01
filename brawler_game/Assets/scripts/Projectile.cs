using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// the bullet that all guns shoot
public class Projectile : NetworkBehaviour {

	public int damage;
	public int speed;
	public int dir;
	// the player who shot the bullet
	public GameObject shooter;

	// speed of the projectile
	protected int STANDARD_SPEED = 80;


	public int getDamage() {
		return damage;
	}

	// Use this for initialization
	void Start () {
		speed = STANDARD_SPEED;
	
	}
	
	// Update is called once per frame
	void Update () {
		// if dir == -1 projectile will move backwards
		transform.position = new Vector3 (transform.position.x + (dir * speed * Time.deltaTime), transform.position.y, transform.position.z);

	}

	// This is the 'onCollision' method
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		// if collide with map boundary, destroy the projectile
		if (coll.gameObject.tag == "Bounds") {
			Destroy (this.gameObject);
		}
		// if collide with player, subtract damage from player's health
		if (coll.gameObject.tag == "Player") {
			// don't want to collide with the player who fired the projectile
			if (coll.gameObject != shooter) {
				coll.gameObject.GetComponent<Player>().takeDamageFromProjectile(this.gameObject);
				Destroy (this.gameObject);
			}
		}
	}

	// the equivalent to the constructor
	// initialize a projectile after it has been created
	public void create(Weapon weapon) {
		this.damage = 10;
		this.speed = STANDARD_SPEED;
		// set the direction of the bullet depending on which way weapon is facing
		this.dir = weapon.getDir ();
		// get the size of the player
		// need to spawn the bullet a bit to the side of the player so it doesnt
		// collide with the person that shot it
		float playerColliderSize = FindObjectOfType<Player>().gameObject.GetComponent<BoxCollider2D>().size.x / 2 + 0.5f;
		float projSize = 1;
		// add a small amount to the x distance so bullet does not collide with shooter
		transform.position = new Vector3(weapon.transform.position.x + ((projSize + playerColliderSize) * this.dir), weapon.transform.position.y, weapon.transform.position.z);
		// set the shooter to be the weapon's parent
		shooter = weapon.transform.parent.gameObject;
	}
}
