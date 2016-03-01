using UnityEngine;
using System.Collections;

// first aid kit pickup
// increases player's health
public class firstAidKit : Pickup {

	public override void onPickUp(Player player) {
		// set player's health to the max
		player.setHealth (player.STARTHEALTH);

	}
}
