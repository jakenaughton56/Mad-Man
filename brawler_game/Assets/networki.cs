using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class networki : MonoBehaviour {

	public void a() {
		print ("BUTTONS");
		GameObject[] objs = GameObject.FindObjectsOfType<GameObject> ();

		for (int i = 0; i < objs.Length; i++) {
			Destroy(objs[i]);
		}

	}
}
