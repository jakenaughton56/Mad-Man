using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


// syncs player position, weapon position and shooting
public class PlayerSyncBehaviour : NetworkBehaviour {

	// server automatically syncs this value to all clients when it changes
	[SyncVar] private Vector3 syncPos;
	[SyncVar] private int syncDir;
	[SyncVar] int syncHasFired;
	[SyncVar] int syncHealth;


	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate;
	[SerializeField] int hasFired;

	

	// Update is called once per frame
	void FixedUpdate () {
		// send the player's position to the server
		TransmitPosition ();
		// smooth all non-local player's movement
		lerpPosition ();
	}

	// only happens on player characters that aren't the client
	void lerpPosition() 
	{
		// only lerp if not the client
		if (!isLocalPlayer) {
			Player playerScript = GetComponent<Player>();
			// sync player's health
			playerScript.setHealth(syncHealth);
			// sync player's position
			myTransform.position = Vector3.Lerp (myTransform.position, syncPos, Time.deltaTime * lerpRate);
			// sync player's weapon position
			GameObject playerWeapon = playerScript.getWeapon ();
			// sync player's direction
			playerScript.setDir(syncDir);
			// if player has fired since we last checked
			if (syncHasFired == 1) {
				if (playerWeapon == null) {
				}
				// otherwise if player has a weapon, attack using weapon
				else if (playerWeapon.GetComponent<Weapon> ().id == 1) {
					playerScript.animator.SetInteger("State", Player.STATE_MELEE);
					playerWeapon.GetComponent<Weapon> ().attack ();
				} else {
					playerWeapon.GetComponent<Weapon>().attack();
				}
				// reset flag
				GetComponent<Player>().hasFired = 0;
			}
			// check if player has died
			if (playerScript.getHealth() < 0 && !playerScript.isDead) {
				// if player has died, add a kill to the player that shot the killing projectile
				playerScript.lastHitBy.GetComponent<Player>().addKill();
				playerScript.isDead = true;
			}
		}
	}

	// command is sent to the server - runs on server not client
	// send information about player to the server
	[Command]
	void CmdProvidePositionToServer(Vector3 pos, int playerDir, int hasFired, int health) {
		syncPos = pos;
		syncDir = playerDir;
		syncHasFired = hasFired;
		syncHealth = health;
	}

	[ClientCallback]
	void TransmitPosition() {
		// if client, send all information about the player to the server
		// so that other clients know whats up
		if (isLocalPlayer) {
			int playerDir = GetComponent<Player>().getDir();
			int hasFired = GetComponent<Player>().hasFired;
			int health = GetComponent<Player>().getHealth();
			CmdProvidePositionToServer (myTransform.position, playerDir, hasFired, health);
		}
	}
}
