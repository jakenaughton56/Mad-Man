using UnityEngine;
using System.Collections;

public class joystickPositionEdit : MonoBehaviour {
	public GameObject joystick;
	public GameObject shootButton;
	public GameObject attackButton;

	GameObject j;
	GameObject s;
	GameObject a;
	// Use this for initialization
	void Start () {
		j = Instantiate (joystick);
		s = Instantiate (shootButton);
		a = Instantiate (attackButton);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
