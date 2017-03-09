using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {
	public int powerLevel;
	private bool isSelected = false;
	bool moved;

	public Material defaultMaterial;
	public Material selectedMaterial;

	public int playerOwner = 0;
	private MeshRenderer mesh;
	public Board currentTile = null;



	// Use this for initialization
	void Start () {
		powerLevel = 1;
		isSelected = false;
		moved = false;

		mesh = GetComponent<MeshRenderer> ();
		mesh.material = defaultMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPower(int increase)
	{
		powerLevel += increase;

		MeshRenderer[] spheres = GetComponentsInChildren<MeshRenderer> ();
		for(int i = 0; i < powerLevel && i < spheres.Length; ++i)
		{
			spheres [i].enabled = true;
		}
	}

	public void SetMaterials(Material def, Material sel)
	{
		defaultMaterial = def;
		selectedMaterial = sel;
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

	public bool GetMoved()
	{
		return moved;
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

	public bool CanMoveToTile(Board tile)
	{
		float dist = Vector3.Distance (tile.transform.position, currentTile.transform.position);
		//print ("dist: " + dist);
		if (moved || dist > powerLevel * 10)
			return false;

		return true;
	}
}
