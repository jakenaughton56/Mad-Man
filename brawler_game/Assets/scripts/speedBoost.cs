using UnityEngine;
using System.Collections;

// pickup which increases the player's speed
public class speedBoost : Pickup {
	
	public override void onPickUp(Player player) {
		// set player's health to the max
		player.increaseSpeed (40, 5);
		
	}
}
