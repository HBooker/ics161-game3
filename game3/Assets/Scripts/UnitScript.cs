using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {
	public int level;
	bool selected;
	bool moved;

	// Use this for initialization
	void Start () {
		level = 1;
		selected = false;
		moved = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseDown() {
		if (Input.GetMouseButtonDown (0)) {
			if (GameObject.FindWithTag ("unit")) {
				selected = !selected;
				if (selected)
					selected = selected; //change later
				else
					selected = selected; // change later
			}
		}
	}

	public void SetMoved (bool has_moved) {
		moved = has_moved;
	}

	public void SetSelected (bool is_selected) {
		selected = is_selected;
	}
}
