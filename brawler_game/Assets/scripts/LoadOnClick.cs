using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	// load a given scene	
	public void LoadScene(int level)
	{
		Application.LoadLevel(level);
	}
}