using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {
	public int powerLevel;
	private bool moved = false;

	public Material defaultMaterial;
	public Material selectedMaterial;
	public int playerOwner = 0;
	public Board currentTile = null;

	private float moveRange = 1.5f;
	private MeshRenderer mesh;
	private Animator anim;

	// Use this for initialization
	void Start () {
		powerLevel = 1;
		moved = false;

		mesh = GetComponent<MeshRenderer> ();
		mesh.material = defaultMaterial;
		anim = GetComponent<Animator> ();
		//anim.Stop ();
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

	public void SetMoved (bool has_moved) {
		moved = has_moved;
		anim.SetBool ("moveable", !moved);
	}

	public bool GetMoved()
	{
		return moved;
	}

	public void Select()
	{
		mesh.material = selectedMaterial;
	}

	public void Deselect()
	{
		mesh.material = defaultMaterial;
	}

	public bool CanMoveToTile(Board tile)
	{
		float dist = Vector3.Distance (tile.transform.position, currentTile.transform.position);

		if(tile.occupyingUnit != null)
		{
			if((tile.occupyingUnit.playerOwner != playerOwner && tile.occupyingUnit.powerLevel > powerLevel) || tile.occupyingUnit.playerOwner == playerOwner)
				return false;
		}

		if (moved || dist > moveRange * 10)
			return false;

		return true;
	}
}
