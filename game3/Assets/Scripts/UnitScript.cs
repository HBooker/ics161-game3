using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {
	public int level;
	private bool isSelected = false;
	bool moved;

	public Material defaultMaterial;
	public Material selectedMaterial;

	private MeshRenderer mesh;
	public Board currentTile = null;

	// Use this for initialization
	void Start () {
		level = 1;
		isSelected = false;
		moved = false;

		mesh = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnLeftMouseDown() {
		if (GameObject.FindWithTag ("unit")) {
			isSelected = !isSelected;
			if (isSelected)
				isSelected = isSelected; //change later
			else
				isSelected = isSelected; // change later
		}

	}

	public void SetMoved (bool has_moved) {
		moved = has_moved;
	}

	public void Select()
	{
		mesh.material = selectedMaterial;
		isSelected = true;
	}

	public void Deselect()
	{
		mesh.material = defaultMaterial;
		isSelected = false;
	}
}
