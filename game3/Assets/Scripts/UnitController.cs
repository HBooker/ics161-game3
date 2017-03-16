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
	private SpriteRenderer unit_sprite;

	void Awake()
	{	
		mesh = GetComponent<MeshRenderer> ();
		anim = GetComponentInChildren<Animator> ();
		unit_level = GetComponentInChildren<TextMesh> ();
		unit_sprite = GetComponentInChildren<SpriteRenderer> ();
		FlipSprite ();
	}

	void Start () {
		powerLevel = 1;
		hasMoved = false;
		mesh.material = defaultMaterial;
	}

	void Update () {
		if (!hasMoved)
			anim.SetBool ("isMoveable", true);
		else
			anim.SetBool ("isMoveable", false);
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
		anim.SetBool ("isMoveable", !hasMoved);
	}

	public bool GetMoved()
	{
		return hasMoved;
	}

	public void Select()
	{
		anim.SetBool ("isMoveable", false);
		mesh.material = selectedMaterial;
	}

	public void Deselect()
	{
		anim.SetBool ("isMoveable", !hasMoved);
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

	public void SetAttacking () {
		StartCoroutine (AttackAnimation ());
	}
	IEnumerator AttackAnimation () {
		anim.SetBool ("isAttacking", true);
		yield return new WaitForSeconds(1.5f);
		anim.SetBool ("isAttacking", false);
	}

	public void SetDead () {
		anim.SetBool ("isDead", true);
	}
	public void SetHit () {
		StartCoroutine (HitAnimation ());
	}

	IEnumerator HitAnimation () {
		anim.SetBool ("isHit", true);
		yield return new WaitForSeconds(1.5f);
		anim.SetBool ("isHit", false);
	}
	public void FlipSprite() {
		if (playerOwner == 2) {
			unit_sprite.flipX = false;
			unit_sprite.transform.localPosition.Set (-0.059f, 0.51f, 0.0f);
		}
	}
}
