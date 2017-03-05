using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoard : Board {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		spawn_tile = true;
		occupied = false;
	}
	private void OnMouseDown() {
		if (Input.GetMouseButtonDown (0)) {
			if (GameObject.FindWithTag ("spawnboard")) {
				selected = !selected;
				if (selected)
					Debug.Log("selected"); //change later
				else
					Debug.Log("not selected"); // change later
			}
		}
	}
		
}
