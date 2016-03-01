using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// randomly spawns items in the world

public class itemSpawner : NetworkBehaviour {
	// height at which object should spawn
	private float MAXSPAWNHEIGHT = 30;

	// spawnable items (weapons, powerups)
	public GameObject[] items;

	// left and right bounds of the map
	public GameObject leftBound;
	public GameObject rightBound;
	// map ground
	public GameObject ground;


	// generate a random position within the map
	private Vector3 generateItemLocation() {
		// range for item to spawn
		float xMin = leftBound.transform.position.x;
		float xMax = rightBound.transform.position.x;
		float yMin = ground.transform.position.y;

		float xPos = Random.Range (xMin, xMax);
		float yPos = Random.Range (yMin, MAXSPAWNHEIGHT);
		CmdSpawn ();
		Vector3 spawnPos = new Vector3 (xPos, yPos, -3);
		return spawnPos;
	}

	// return a random gameobject to spawn
	private GameObject generateRandomItem() {
		return items[Random.Range(0, items.Length)];
	}

	// command runs on the server
	// this way any items spawned will be automatically synced
	// to all clients in the game
	[Command]
	public void CmdSpawn() {
		// change the '100' to a higher value for more infrequent spawns
		if (Random.Range (0, 100) == 4) {
			// get the position for the item to spawn
			Vector3 pos = generateItemLocation();
			// choose a random item to spawn
			GameObject itemToSpawn = generateRandomItem();
			// spawn the item on the server
			GameObject newItem = (GameObject)Instantiate (itemToSpawn, pos, Quaternion.identity);
			// now spawn the item on all the clients
			NetworkServer.Spawn (newItem);
		}
	}

	void FixedUpdate() {
		// every fixed update, randomly choose to spawn an item or not
		CmdSpawn ();
	}
}
