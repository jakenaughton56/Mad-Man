using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// controls the behaviour of the main menu
public class mainMenuController : MonoBehaviour {

	// the name entry popup
	public GameObject nameEntryPanel;
	// to show the player's name
	public Text playerName;
	// to update DB
	public GameObject Database;

	// Use this for initialization
	void Start () {
		nameEntryPanel.SetActive (false);;
		// if the player doesn't have a name, bring up name choosing screen
		if (!playerHasName ()) {
			nameEntryPanel.SetActive (true);
		} else {
			// player has set name, so show their name
			playerName.text += PlayerPrefs.GetString("Name");
		}
	
	}

	// check whether the player has saved their name
	bool playerHasName() {
		// check bool to see if player has set name previously
		int setName = PlayerPrefs.GetInt ("SetName");
		if (setName == 0) {
			return false;
		} return true;
	}

	// called when the player presses the 'done' button
	// in the enter name screen
	public void onPlayerEnteredName() {

		// create new database object
		GameObject db = Instantiate (Database);
		DBUtils dbScript = db.GetComponent<DBUtils> ();


		// get the name the user has entered
		string enteredName = nameEntryPanel.transform.FindChild("InputField").GetComponent<InputField>().text;
		print (enteredName);
		// store flag and users entered name in their playerprefs
		// this will be kept in between sessions
		PlayerPrefs.SetInt ("SetName", 1);
		PlayerPrefs.SetString ("Name", enteredName);

		// create an entry for this player in the database
		dbScript.createPlayer (enteredName);


		// entered a name, no longer need the name entry screen, so disable it
		nameEntryPanel.SetActive (false);

		// update the name text on the main menu
		playerName.text += PlayerPrefs.GetString("Name");

	}
}
