using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// controls the scrollable statistic displayer
public class statHolderController : MonoBehaviour {

	// statFrame prefab
	public GameObject statFrame;
	// to store the height of a stat frame
	// this will determine how far down the new statFrame needs to be created
	private float statFrameHeight;
	// how many stat frames we have
	public int numFrames;

	void Start() {
		statFrameHeight = statFrame.GetComponent<RectTransform> ().rect.height;
		numFrames = 0;
	}


	// add a new stat frame underneath the current one
	public GameObject addNewStatFrame(string field1, string field2, string field3, string field4) {
		// create new stat filed
		GameObject newStat = (GameObject)Instantiate (statFrame);
		newStat.transform.SetParent(this.transform);
		RectTransform rt = newStat.GetComponent<RectTransform> ();
		// set position of new field to be one place down from last field created
		rt.offsetMin = new Vector2 (0, -statFrameHeight * numFrames);
		rt.offsetMax = new Vector2 (0, -statFrameHeight * numFrames);
		numFrames++;

		// now fill the text fields
		updateFields (newStat, field1, field2, field3, field4);
		return newStat;
	}

	// update the text in each of the four fields of a panel
	public void updateFields(GameObject panel, string field1, string field2, string field3, string field4) {
		foreach (Transform child in panel.transform) {
			// each child is a panel, with a single child that is a text 
			string fieldName = child.name;
			// get the text component of the child
			Text fieldText = child.GetChild(0).GetComponent<Text>();
			// fill each field with the correct value
			if (fieldName == "field1")
				fieldText.text = field1.Replace("\"", "");
			else if (fieldName == "field2")
				fieldText.text = field2.Replace("\"", "");
			else if (fieldName == "field3")
				fieldText.text = field3.Replace("\"", "");
			else if (fieldName == "field4")
				fieldText.text = field4.Replace("\"", "");
		}	
	}
}
