using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	RectTransform rt;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		//print (rt.localPosition);
		rt.localPosition = new Vector3 (rt.localPosition.x, rt.localPosition.y - 0.1f, rt.localPosition.z);
	
	}
}
