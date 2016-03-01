using UnityEngine;
using System.Collections;

// game object that will be passed between scenes in order to keep track of how many players joined the lobby
public class playerCount : MonoBehaviour {
	private int numPlayers;
	// Use this for initialization
	void Start () {
		numPlayers = 0;
	}

	// add one to the counter
	public void addPlayer() {
		numPlayers++;
	}

	// return the number of players that joined the lobby
	public int getNumPlayers() {
		return numPlayers;
	}

	// find the player counter in the scene
	public static GameObject findCounter() {
		return GameObject.Find("playerCount");
	}
}
