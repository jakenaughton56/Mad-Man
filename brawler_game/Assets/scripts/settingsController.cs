using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// controls the settings screen
public class settingsController : MonoBehaviour {

	// the ui to show when we prompt the user for a new username
	public GameObject enterNewNameScreen;

	// to manage the database
	public GameObject Database;

	// Use this for initialization
	void Start () {
		// disable the new name screen, it will only be activated when the new name button
		// is pressed
		enterNewNameScreen.SetActive (false);
	}

	// bring up screen to allow user to enter their name
	public void enterNewName() {
		enterNewNameScreen.SetActive (true);
		// fill input field with current name
		enterNewNameScreen.transform.FindChild("InputField").GetComponent<InputField>().text = PlayerPrefs.GetString("Name");
	}
	
	// called when the player presses the 'done' button
	// in the enter name screen
	public void onPlayerEnteredName() {
		// create new database object
		GameObject db = Instantiate (Database);
		DBUtils dbScript = db.GetComponent<DBUtils> ();
		// get the name the user has entered
		string enteredName = enterNewNameScreen.transform.FindChild("InputField").GetComponent<InputField>().text;
		print (enteredName);
		// store flag and users entered name in their playerprefs
		// this will be kept in between sessions
		PlayerPrefs.SetInt ("SetName", 1);
		PlayerPrefs.SetString ("Name", enteredName);
		// create an entry for this player in the database
		dbScript.createPlayer (enteredName);
		// entered a name, no longer need the name entry screen, so disable it
		enterNewNameScreen.SetActive (false);

	}

	// called when the edit joystick button is pressed
	public void editJoystick() {
		// load the joystick editing level 
		Application.LoadLevel (6);
	}
}
