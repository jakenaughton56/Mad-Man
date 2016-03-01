using UnityEngine;
using System.Collections;

public abstract class Pickup : Item {

	// a pickup item
	// changes player stats when picked up
	public abstract void onPickUp (Player player);
}
