// set of functions that send data to the game database

using UnityEngine;
using System.Collections;

public class DBUtils : MonoBehaviour {

	// send a player's stats to the database
	public void upStats(string playerName, int won, int kills, int deaths) {
		string dataURL = "http://crackfox.hostoi.com/uploadStats.php?"
			+ "pid=" + playerName 
				+ "&win=" + won
				+ "&kills=" + kills
				+ "&deaths=" + deaths;

		StartCoroutine(runDBQuery(dataURL));
	}

	// run a database query
	public IEnumerator runDBQuery(string query)
	{
		print (query);
		WWW returnText = new WWW (query);
		yield return returnText;
		print (returnText.text);

	}

	// create an entry for a new player
	public void createPlayer(string name) {
		string dataURL = "http://crackfox.hostoi.com/createPlayer.php?name=" + name;
		StartCoroutine(runDBQuery(dataURL));
	}

}
