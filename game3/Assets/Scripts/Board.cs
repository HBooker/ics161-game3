﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	public bool isSpawner = false;
	public bool selected = false;
	public float unitYOffset = 4.0f;

	public int player_on_tile = 0;
	public int player_owner = 0;

	public UnitScript occupyingUnit = null;

	void Start () 
	{
		
	}

	void Update ()
	{
		if (selected && Input.GetKeyDown (KeyCode.BackQuote)) {
			Occupy (FindObjectOfType<UnitScript>());
		}
	}

	public void Occupy(UnitScript unit)
	{
		if(IsOccupied())
		{
			//do something
		}
		else
		{
			Vector3 unitPosition = new Vector3 (transform.position.x, transform.position.y + unitYOffset, transform.position.z);
			unit.transform.position = unitPosition;
			occupyingUnit = unit;
			unit.currentTile = this;
		}
	}

	public void Vacate()
	{
		occupyingUnit.currentTile = null;
		occupyingUnit = null;
	}

	public bool IsOccupied () 
	{
		return (occupyingUnit != null);
	}




	public void SetPlayerOnTile (int player_num) {
		player_on_tile = player_num;
		//1 = player 1, 2 = player 2, etc
	}
	public int GetPlayerOnTile () {
		return player_on_tile;
	}

	public void SetOwner (int player) {
		player_owner = player;
	}
	public int GetOwner () {
		return player_owner;
	}

	public bool IsSelected () {
		return selected;
	}

	public void SetSelected (bool is_selected) {
		selected = is_selected;
	}
}
