using UnityEngine;
using System.Collections;

// controls the on-screen joystick and buttons
public class joystickController : MonoBehaviour {
	// bounds of the joystick
	public GameObject bounds;
	// shooting and jumping buttons
	public GameObject attack;
	public GameObject jump;
	// size of the joystick
	private float boundSizeX;
	private float boundSizeY;
	private CircleCollider2D collider;
	bool touchedLastFrame;
	// to store the last touch position that effected the joystock
	Vector3 lastTouchPos;
	public bool isAttacking;
	public bool isJumping;

	// the game's camera
	private Camera gameCam;
	

	// Use this for initialization
	void Start () {
		// get the camera
		gameCam = Camera.main;
		boundSizeX = bounds.transform.lossyScale.x;
		boundSizeY = bounds.transform.lossyScale.y;
		collider = GetComponent<CircleCollider2D>();
		// centre the joystick in the middle of the bounding circle
		transform.position = bounds.transform.position;
		touchedLastFrame = false;

		// if player has set custom controller locations, update them accordingly
		if (PlayerPrefs.GetInt ("joystickEdited") == 1) {
			// calculate ratio between this scene's camera size and the size of the camera 
			// in the scene where we set the joystick
			float scale = Camera.main.orthographicSize / PlayerPrefs.GetFloat("cameraSize");
			setJoystickPos(scale);
			setAttackPos(scale);
			setJumpPos(scale);
		}
	}

	// set the joystick to be at saved position
	private void setJoystickPos(float scale) {
		Vector3 pos = new Vector3 (PlayerPrefs.GetFloat ("joystickX"), PlayerPrefs.GetFloat ("joystickY"), PlayerPrefs.GetFloat ("joystickZ"));
		Vector3 cameraPos = gameCam.transform.position;
		pos = new Vector3 (pos.x + cameraPos.x, pos.y + cameraPos.y, pos.z);
		transform.position = pos;
		//transform.localScale = new Vector3(scale/4, scale/4, 1);
		bounds.transform.localScale = new Vector3(scale, scale, 1);
		bounds.transform.position = pos * scale;
	}
	// set the attack button to be at saved position
	private void setAttackPos(float scale){
		Vector3 pos = new Vector3 (PlayerPrefs.GetFloat ("attackX"), PlayerPrefs.GetFloat ("attackY"), PlayerPrefs.GetFloat ("attackZ"));
		Vector3 cameraPos = gameCam.transform.position;
		pos = new Vector3 (pos.x + cameraPos.x, pos.y + cameraPos.y, pos.z);
		attack.transform.position = pos * scale;
		attack.transform.localScale = new Vector3(scale/2, scale/2, 1);

	}
	// set the jump button to be at saved position
	private void setJumpPos(float scale) {
		Vector3 pos = new Vector3 (PlayerPrefs.GetFloat ("jumpX"), PlayerPrefs.GetFloat ("jumpY"), PlayerPrefs.GetFloat ("jumpZ"));
		Vector3 cameraPos = gameCam.transform.position;
		pos = new Vector3 (pos.x + cameraPos.x, pos.y + cameraPos.y, pos.z);
		jump.transform.position = pos * scale;
		jump.transform.localScale = new Vector3(scale/2, scale/2, 1);
	}
	// Update is called once per frame
	void Update () {
		// update flags
		isAttacking = false;
		isJumping = false;
		bool wasClicked = false;
		try {
			Touch myTouch = Input.GetTouch(0);
			
			Touch[] myTouches = Input.touches;
			for(int i = 0; i < Input.touchCount; i++)
			{
				Vector3 touchPos = (Camera.main.ScreenToWorldPoint(myTouches[i].position));
				float distance = Vector2.Distance(new Vector2(touchPos.x, touchPos.y), new Vector2(transform.position.x, transform.position.y));
				float distanceFromLastTouch = Vector2.Distance (new Vector2(touchPos.x, touchPos.y), new Vector2(lastTouchPos.x, lastTouchPos.y));
				// check if either the attack or jump buttons are being pressed

				// move the joystick to the touch position if either:
				//	the touch position was inside the joystick button
				//  the touch position was within 2 units from the last touch position
				if (distance < boundSizeX || distanceFromLastTouch < 2) {
					lastTouchPos = touchPos;
					wasClicked = true;
					checkPos (touchPos);
				}
				distance = 0;
				distanceFromLastTouch = 0;

				// check for touches on the jump and attack buttons
				if (jump.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					isJumping = true;
					
				}
				if (attack.GetComponent<CircleCollider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					isAttacking = true;
					
				}
			}
		}
		// if the joystick was moved, reset it to its original position
		catch {
			if (Input.touchCount == 0) {
				transform.position = bounds.transform.position;
			}
			}
	}

	// get the current x direction of the joystick, normalized
	public float getDirX() {
		return ((transform.position.x - bounds.transform.position.x) / boundSizeX);
	}

	// get the current y direction of the joystick, normalized
	public float getDirY() {
		return ((transform.position.y - bounds.transform.position.y) / boundSizeY);
	}

	// check if new position of inner joystick is within the bounds of the outer one
	// if not, place it on border
	void checkPos(Vector3 newPos) {

		float distFromOrigin = (Vector2.Distance (new Vector2 (newPos.x, newPos.y), new Vector2 (bounds.transform.position.x, bounds.transform.position.y)));
		// if we are within bounds, move the joystick
		if (distFromOrigin < boundSizeX) {
			transform.position = new Vector3 (newPos.x, newPos.y, 0);
		} 
		// too far from origin, so place joystick on bounds but still in the direction of the player's finger
		// too keep direction
		else {
			// translate position to (0,0,0)
			Vector3 translated = new Vector3(newPos.x - bounds.transform.position.x, newPos.y - bounds.transform.position.y, 0);
			// calculate vector in same direction as users finger but within bounds of joystick circle
			Vector3 fixedLength = new Vector3((translated.x / distFromOrigin) * boundSizeX,(translated.y / distFromOrigin) * boundSizeY, 0);
			// now apply the new position
			transform.position = new Vector3(fixedLength.x + bounds.transform.position.x, fixedLength.y + bounds.transform.position.y, 0);
		}
	}

	// check if a press in on a button
	bool checkForButtonPress(GameObject button, Vector3 touchPos) {
		// raycast from touch to see if it is on button
		RaycastHit2D hit = Physics2D.Raycast (new Vector2(touchPos.x, touchPos.y), Vector2.zero);
		// if hit something and it was the button we are checking for, return true
		if (hit != null && hit.collider.gameObject == button) {
			return true;
		}
		// was not touched
		return false;
	}
}
