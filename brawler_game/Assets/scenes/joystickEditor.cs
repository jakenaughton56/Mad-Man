using UnityEngine;
using System.Collections;

// allow the user to 'drag' the joystick and buttons around on the screen
// so they can place them wherever they want
public class joystickEditor : MonoBehaviour {

	GameObject attackButton;
	GameObject jumpButton;
	GameObject joystick;
	// Use this for initialization
	void Start () {
		// get each of the components
		attackButton = transform.Find ("attackButton").gameObject;
		jumpButton = transform.Find ("jumpButton").gameObject;
		joystick = transform.Find ("Joystick").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		// iterate through all the touches
		try {
			Touch myTouch = Input.GetTouch(0);
			
			Touch[] myTouches = Input.touches;
			for(int i = 0; i < Input.touchCount; i++)
			{

				Vector3 touchPos = (Camera.main.ScreenToWorldPoint(myTouches[i].position));
				// if touched a button/joystick, move that object to the position of the touch
				// this allows the player to drag them around
				if (attackButton.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					attackButton.transform.position = new Vector3(touchPos.x, touchPos.y, 0);
				}
				if (jumpButton.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					jumpButton.transform.position = new Vector3(touchPos.x, touchPos.y, 0);
				}
				if (joystick.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					joystick.transform.position = new Vector3(touchPos.x, touchPos.y, 0);
				}
			}
		}
		catch {
		}
	}

	// when the done button is pressed
	public void onDonePress() {
		// save the new positions
		saveJoystickPos ();
		saveAttackPos ();
		saveJumpPos ();
		// set a flag so we know the joystick has been edited
		PlayerPrefs.SetInt ("joystickEdited", 1);
		// remember the camera size
		PlayerPrefs.SetFloat ("cameraSize", Camera.main.orthographicSize);
		Application.LoadLevel (6);
	}

	// save the position of the joystick
	private void saveJoystickPos() {
		PlayerPrefs.SetFloat ("joystickX", joystick.transform.position.x);
		PlayerPrefs.SetFloat ("joystickY", joystick.transform.position.y);
		PlayerPrefs.SetFloat ("joystickZ", joystick.transform.position.z);
	}

	// save the position of the attack button
	private void saveAttackPos() {
		PlayerPrefs.SetFloat ("attackX", attackButton.transform.position.x);
		PlayerPrefs.SetFloat ("attackY", attackButton.transform.position.y);
		PlayerPrefs.SetFloat ("attackZ", attackButton.transform.position.z);
	}

	// save the position of the jump button
	private void saveJumpPos() {
		PlayerPrefs.SetFloat ("jumpX", jumpButton.transform.position.x);
		PlayerPrefs.SetFloat ("jumpY", jumpButton.transform.position.y);
		PlayerPrefs.SetFloat ("jumpZ", jumpButton.transform.position.z);
	}
}
