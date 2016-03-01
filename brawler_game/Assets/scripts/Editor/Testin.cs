using UnityEngine;

using System.Collections;

using NUnit.Framework;



public class Testing : MonoBehaviour {

	[Test]
	public void gunCooldownIncreasesAfterShoot(){

		GameObject gun = GameObject.Find ("Gun");
		SimpleGun script = gun.GetComponent<SimpleGun> ();
		script.init ();
		script.attack ();
		DestroyImmediate(GameObject.Find("Projectile(Clone)"));
		Assert.Greater(script.getCooldown(), 0);
	}



	[Test]
	public void gunAmmoDecreasesAfterShoot(){

		GameObject gun = GameObject.Find ("Gun");

		SimpleGun script = gun.GetComponent<SimpleGun> ();
		script.init ();
		int count = script.getAmmo ();

		script.attack ();
		int newcount = script.getAmmo ();

		DestroyImmediate(GameObject.Find("Projectile(Clone)"));

		Assert.AreEqual (count - 1, newcount);

	}

	[Test]
	public void machineGunCooldownIncreasesAfterShoot(){
		
		GameObject gun = GameObject.Find ("gun2");
		machineGun script = gun.GetComponent<machineGun> ();
		script.init ();
		script.attack ();
		DestroyImmediate(GameObject.Find("Projectile(Clone)"));
		Assert.Greater(script.getCooldown(), 0);
	}
	
	
	
	[Test]
	public void machingGunAmmoDecreasesAfterShoot(){
		
		GameObject gun = GameObject.Find ("gun2");
		
		machineGun script = gun.GetComponent<machineGun> ();
		script.init ();
		int count = script.getAmmo ();
		
		script.attack ();
		int newcount = script.getAmmo ();

		DestroyImmediate (GameObject.Find ("Projectile(Clone)"));
		
		Assert.AreEqual (count - 1, newcount);

	}

}