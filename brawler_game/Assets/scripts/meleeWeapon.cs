using UnityEngine;
using System.Collections;

// the default weapon when the player has no gun
public class meleeWeapon : Weapon {
	
	// the 'projectile' that the player shoots
	public GameObject fist;
	
	// Use this for initialization
	void Start () {
		base.ammo = 1;
		base.cooldown = 0;
		base.dir = 1;
		base.id = 1;
	}
	
	
	public override void attack() {
		// if we have enough ammo and cooldown is finished, then shoot
		if (base.ammo > 0 && base.cooldown <= 0) {
			GameObject newFist = (GameObject)Instantiate(fist);
			// set new projectiles position to be gun's position
			newFist.GetComponent<Projectile>().create(this);

			// destroy the new projectile after short amount of time
			// so it only hurts other players if they are near
			Destroy(newFist,0.02f);
			
			// play gun sound
			
			// add to cooldown
			base.cooldown += 0.5f;
		}
	}
}