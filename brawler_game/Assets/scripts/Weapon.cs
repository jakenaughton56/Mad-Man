using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// superclass for all weapons
public abstract class Weapon : Item {

	// how much damage bullets from the weapon do
	protected int damage;
	// how much ammo the weapon has
	protected int ammo;
	public float cooldown;
	protected int dir;
	public int id;
	

	public int getDamage() {
		return damage;
	}

	
	// Update is called once per frame
	void Update () {
		// cool down the gun
		if (cooldown > 0) {
			cooldown -= (float)Time.deltaTime;
		}
		// destroy the gun if no ammo left
		if (ammo <= 0) {
			Destroy(this.gameObject);
		}
	}
	
	// get the amount of ammo left
	public int getAmmo(){
		return ammo;
	}

	// get the current cooldown value
	public float getCooldown(){
		return cooldown;
	}
	
	// change weapon's direction
	public void changeDir(int newDir) {
		dir = newDir;
	}

	// get the direction the weapon is facing
	public int getDir() {
		return dir;
	}

	// attack function to be overrided by all weapon 
	// subclasses
	public abstract void attack();

}
