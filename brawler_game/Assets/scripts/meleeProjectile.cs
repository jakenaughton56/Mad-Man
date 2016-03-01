using UnityEngine;
using System.Collections;

// subclass of the normal projectile
// deletes itself after travelling more than 5 game units
public class meleeProjectile : Projectile {

	// max distance the projectile should travel
	private int MAXDIST = 5;
	// how far it has currently travelled
	private float distTravelled;
	// its current position
	private Vector3 lastPos;

	// Use this for initialization
	void Start () {
		lastPos = transform.position;
		speed = base.STANDARD_SPEED;
		distTravelled = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		distTravelled += Vector3.Distance (lastPos, transform.position);
		lastPos = transform.position;

		if (distTravelled > MAXDIST) {
			Destroy (this.gameObject);
		}

		// if dir == -1 projectile will move backwards
		transform.position = new Vector3 (transform.position.x + (dir * speed * Time.deltaTime), transform.position.y, transform.position.z);
		
	}
}
