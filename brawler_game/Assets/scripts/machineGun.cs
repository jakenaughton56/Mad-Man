using UnityEngine;
using System.Collections;

// sub-class of Weapon
// a machine gun
public class machineGun : Weapon {
	
	// the projectile the gun shoots
	public Projectile projectile;
	
	// Use this for initialization
	void Start () {
		base.ammo = 100;
		base.cooldown = 0;
		
	}

	public void init(){
		Start ();
	}


	
	// Update is called once per frame
	void Update () {
		// cool down the gun
		if (cooldown > 0) {
			base.cooldown -= (float)Time.deltaTime;
		}
		if (ammo <= 0) {
			Destroy(this.gameObject);
		}
	}
	
	public override void attack() {
		
		// if we have enough ammo and cooldown is finished, then shoot
		if (base.ammo > 0 && base.cooldown <= 0) {
			Projectile newProj = (Projectile)Instantiate(projectile);
			// set new projectiles position to be gun's position
			newProj.create(this);
			// play gun sound
			base.ammo --;
			// add to cooldown
			base.cooldown += 0.05f;
		}
		// otherwise, play empty gun noise
		else {
			// play the empty gun noise if we ever get one
		}
	}
	
}

