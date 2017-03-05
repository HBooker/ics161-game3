using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
	public GameObject this_board;
	public bool spawn_tile;
	public bool selected;
	public bool occupied;
	public int player_on_tile;
	public int player_owner;
	// Use this for initialization
	void Start () {
		this_board = GetComponent<GameObject> ();
		spawn_tile = false;
		occupied = false;
		selected = false;
		player_on_tile = 0;
		player_owner = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsSpawn () {
		return spawn_tile;
	}

	public void SetOccupied (bool is_occupied) {
		occupied = is_occupied;
	}

	public bool GetOccupied () {
		return occupied;
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
