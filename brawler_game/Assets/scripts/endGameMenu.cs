using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// controls the end game menu that will pop up each time a game is finished
public class endGameMenu : MonoBehaviour {

	// 'you won' / 'you lost'
	public Text outcome;
	// counts for kills and deaths
	public Text numKills;
	public Text numDeaths;

	void Start() {
		// get the number of kills and deaths, as well as the outcome 
		// for the player for the most recent game (the one that just finished)
		int kills = PlayerPrefs.GetInt ("numKills");
		int deaths = PlayerPrefs.GetInt ("numDeaths");
		int won = PlayerPrefs.GetInt ("won");

		// update screen to show these stats
		if (won == 1) {
			setOutcome ("You won!");
		} else {
			setOutcome ("You lost!");
		}
		setNumKills (kills);
		setNumDeaths (deaths);

	}

	// change the outcome string, ie 'you won'
	public void setOutcome(string text) {
		outcome.text = text;
	}
	// update kill counter
	public void setNumKills(int kills) {
		numKills.text += kills.ToString ();
	}
	// update death counter
	public void setNumDeaths(int deaths) {
		numDeaths.text += deaths.ToString ();
	}

}
