using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

// fetches stats from the database and displays them on the screen

public class statFetcher : MonoBehaviour {

	public GameObject statHolder;
	// an array of all the stat panels we have created
	private List<GameObject> statPanels;
	// the title bar
	public GameObject titles;

	void Start() {
		// initialize stat panel array
		statPanels = new List<GameObject> ();
	}

	// fetch all stats, including kills, deaths and k/d ratio
	public void getKDStats() {
		StartCoroutine(getStatsCoroutine('k'));
	}

	// fetch all stats, including wins, losses and w/l ratio
	public void getWLStats() {
		StartCoroutine(getStatsCoroutine('w'));
	}

	// get game stats from database
	// statType - either k for kill/deaths or w for wins/losses
	public IEnumerator getStatsCoroutine(char statType)
	{
		string dataURL = "http://crackfox.hostoi.com/getStats.php";
		WWW returnText = new WWW(dataURL);
		yield return returnText;

		// clear the screen of all stat panels to make room for the new ones
		for (int each = 0; each < statPanels.Count; each++) {
			GameObject curr = statPanels[each];
			Destroy(curr);
		}

		// create list to store the stat panels
		statPanels = new List<GameObject>();
		// reset stat frame count in the other script
		statHolder.GetComponent<statHolderController> ().numFrames = 0;

		// parse the returned text into a json object
		JSONObject js = new JSONObject (returnText.text);

		string np = js ["numPlayers"].ToString();
		int numPlayers = 1000;
		int i = 0;
		// add a stat panel for each player
		while (i < numPlayers) {
			// if we are getting the kill/death stats
			if (statType == 'k') {
				// first update the titles in the title bar
				statHolder.GetComponent<statHolderController>().updateFields(titles, "Player", "Kills", "Deaths", "K/D");
				string username =  (js[i.ToString()]["Username"]).ToString().Replace("\"", "");
				string kills =  (js[i.ToString()]["Kills"]).ToString().Replace("\"", "");
				string deaths =  (js[i.ToString()]["Deaths"]).ToString().Replace("\"", "");
				float ratio = (float)Int32.Parse (kills) / (float)Int32.Parse (deaths);
				// add the stat panel to the screen
				GameObject newStatPanel = statHolder.GetComponent<statHolderController>().addNewStatFrame(username, kills, deaths, ratio.ToString("0.00"));
				statPanels.Add (newStatPanel);
				i++;
			} 
			// if we are getting the win/loss stats
			else if (statType == 'w') {
				statHolder.GetComponent<statHolderController>().updateFields(titles, "Player", "Wins", "Losses", "W/L");
				string username =  (js[i.ToString()]["Username"]).ToString().Replace("\"", "");
				string wins =  (js[i.ToString()]["Wins"]).ToString().Replace("\"", "");
				string losses =  (js[i.ToString()]["Losses"]).ToString().Replace("\"", "");
				float ratio = (float)Int32.Parse (wins) / (float)Int32.Parse (losses);
				// add the stat panel to the screen
				GameObject newStatPanel = statHolder.GetComponent<statHolderController>().addNewStatFrame(username, wins, losses, ratio.ToString("0.00"));
				statPanels.Add (newStatPanel);
				i++;
			}
		}

	}

	// convert a string to integer (int.parse() wasnt working for some reason)
	private int stoi(string s) {
		int i = s.Length - 1;
		int pow = 0;
		double sum = 0;
		while (i >= 0) {
			char curr = s[i];
			sum += Math.Pow(10, pow) * (curr - '0');
			i--;
			pow++;
		}
		return (int)sum;
	}



}