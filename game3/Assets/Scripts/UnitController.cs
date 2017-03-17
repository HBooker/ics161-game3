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
	private Animator unitAnim;
	private Animator invAnim;
	private TextMesh unit_level;
	private SpriteRenderer unit_sprite;

	void Awake()
	{	
		mesh = GetComponent<MeshRenderer> ();
		Animator[] anims = GetComponentsInChildren<Animator> ();
		unitAnim = anims[1];
		unit_level = GetComponentInChildren<TextMesh> ();
		unit_sprite = GetComponentInChildren<SpriteRenderer> ();
		invAnim = anims[0];
		FlipSprite ();
	}

	void Start () {
		powerLevel = 1;
		hasMoved = false;
		mesh.material = defaultMaterial;
	}

	void Update () {
		if (!hasMoved) 
			unitAnim.SetBool ("isMoveable", true);
		else
			unitAnim.SetBool ("isMoveable", false);
	}
	public void SetInvincible (bool is_invincible)
	{
		invincible = is_invincible;
		invAnim.SetBool ("isInvulnerable", is_invincible);
	}
	public void AddPower(int increase)
	{
		powerLevel += increase;
		unit_level.text = powerLevel.ToString ();
	}

	public void SetMaterials(Material def, Material sel)
	{
		defaultMaterial = def;
		selectedMaterial = sel;
	}

	public void SetMoved (bool has_moved) {
		hasMoved = has_moved;
		unitAnim.SetBool ("isMoveable", !hasMoved);
	}

	public bool GetMoved()
	{
		return hasMoved;
	}

	public void Select()
	{
		unitAnim.SetBool ("isMoveable", false);
		mesh.material = selectedMaterial;
	}

	public void Deselect()
	{
		unitAnim.SetBool ("isMoveable", !hasMoved);
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
		unitAnim.SetBool ("isAttacking", true);
		yield return new WaitForSeconds(1.5f);
		unitAnim.SetBool ("isAttacking", false);
	}

	public void SetDead () {
		unitAnim.SetBool ("isDead", true);
	}
	public void SetHit () {
		StartCoroutine (HitAnimation ());
	}

	IEnumerator HitAnimation () {
		unitAnim.SetBool ("isHit", true);
		yield return new WaitForSeconds(1.5f);
		unitAnim.SetBool ("isHit", false);
	}
	public void FlipSprite() {
		if (playerOwner == 2) {
			unit_sprite.flipX = false;
			unit_sprite.transform.localPosition.Set (-0.059f, 0.51f, 0.0f);
		}
	}
}
