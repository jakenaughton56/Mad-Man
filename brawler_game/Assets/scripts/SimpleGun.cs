using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// simple pistol weapon
public class SimpleGun : Weapon {

	// the projectile the gun shoots
	public GameObject projectile;

	// Use this for initialization
	void Start () {
		base.ammo = 20;
		base.cooldown = 0;
		base.dir = 1;
	
	}

	public void init(){
		Start ();
	}

	public override void attack() {

		// if we have enough ammo and cooldown is finished, then shoot
		if (base.ammo > 0 && base.cooldown <= 0) {
			GameObject newProj = (GameObject)Instantiate(projectile);
			// set new projectiles position to be gun's position
			newProj.GetComponent<Projectile>().create(this);
			this.ammo -= 1;
			// spawn the bullet on the server
			//NetworkServer.Spawn(newProj.gameObject);

			// play gun sound

			// add to cooldown
			base.cooldown += 0.2f;
		}
		// otherwise, play empty gun noise
		else {
			// play the empty gun noise if we ever get one
		}
	}

}

