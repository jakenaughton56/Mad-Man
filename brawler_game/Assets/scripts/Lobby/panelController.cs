using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class panelController : MonoBehaviour {
	// the level toggles that are stored in this panel
	private List<Toggle> toggles;
	// store whether toggles were on in the last update call
	private List<bool> toggleMemory; 

	// index in list of the toggle that is currently switched on
	int toggleThatIsOnIndex;

	// Use this for initialization
	void Start () {
		toggles = new List<Toggle> ();
		toggleMemory = new List<bool> ();
		foreach (Transform child in transform) {
			toggles.Add (child.gameObject.GetComponent<Toggle> ());
			toggleMemory.Add (false);
		}
		toggleThatIsOnIndex = 0;
		toggles [0].isOn = true;
		toggleMemory[0] = true;
		for (int i = 1; i < toggles.Count; i++) {
			toggles[i].isOn = false;
			toggleMemory[i] = false;
		}
	
	}


}
