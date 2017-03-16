using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimations : MonoBehaviour {
	Animator anim;
	UnitController this_unit;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		this_unit = GetComponentInParent<UnitController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this_unit.GetMoved ())
			anim.SetBool ("IsMoveable", false);
	}
}
