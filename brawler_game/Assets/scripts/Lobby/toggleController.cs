using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class toggleController : MonoBehaviour {

	// the panel the contains this toggle
	public GameObject parentPanel;
	// Update is called once per frame
	public void onPress () {
		foreach (Transform child in parentPanel.transform) {
			print (child.GetComponent<Toggle>().transform.GetChild(1).GetComponent<Text>().text);
			if (child != gameObject.transform) {
				child.gameObject.GetComponent<Toggle> ().isOn = false;
			}
		}
	
	}
}
