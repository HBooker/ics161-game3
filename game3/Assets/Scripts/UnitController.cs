using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	public int powerLevel;
	private bool hasMoved = false;
	public bool invincible = false;

	public Material defaultMaterial;
	public Material selectedMaterial;
	public int playerOwner = 0; 
	public TileController currentTile = null;

	private float moveRange = 1.5f;
	private MeshRenderer mesh;
	private Animator anim;
	private TextMesh unit_level;

	void Awake()
	{
		mesh = GetComponent<MeshRenderer> ();
		anim = GetComponent<Animator> ();
		unit_level = GetComponentInChildren<TextMesh> ();
	}

	void Start () {
		powerLevel = 1;
		hasMoved = false;
		mesh.material = defaultMaterial;
	}

	void Update () {
		
	}
	public void SetInvincible (bool is_invincible)
	{
		invincible = is_invincible;
	}
	public void AddPower(int increase)
	{
		print ("increase " + increase);
		print ("powerLevel " + powerLevel);
		powerLevel += increase;
		print ("powerLevel + increase " + powerLevel);
		unit_level.text = powerLevel.ToString ();
		print (powerLevel.ToString());
	}

	public void SetMaterials(Material def, Material sel)
	{
		defaultMaterial = def;
		selectedMaterial = sel;
	}

	public void SetMoved (bool has_moved) {
		hasMoved = has_moved;
		anim.SetBool ("moveable", !hasMoved);
	}

	public bool GetMoved()
	{
		return hasMoved;
	}

	public void Select()
	{
		anim.SetBool ("moveable", false);
		mesh.material = selectedMaterial;
	}

	public void Deselect()
	{
		anim.SetBool ("moveable", !hasMoved);
		mesh.material = defaultMaterial;
	}

	public bool CanMoveToTile(TileController tile)
	{
		float dist = Vector3.Distance (tile.transform.position, currentTile.transform.position);

		if(tile.occupyingUnit != null && (tile.occupyingUnit.invincible == true || tile.occupyingUnit.playerOwner == playerOwner))
		{
			return false;
		}

		if (hasMoved || dist > moveRange * 10)
			return false;

		return true;
	}
}
