using UnityEngine;
using System.Collections;

public class setAnchors : MonoBehaviour {
	private RectTransform panelRectTransform;

	// Use this for initialization
	void Start () {
		// get the object's rect transform
		panelRectTransform = GetComponent<RectTransform> ();
		//Vector2 bottomLeft = new Vector2 (panelRectTransform.);
		Rect rect = panelRectTransform.rect;

		// Something like this.
		//panelRectTransform.anchorMin = new Vector2(rect.x, rect.y);
		//panelRectTransform.anchorMax = new Vector2(rect.x + rect.width, rect.y + rect.height);
		//panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
