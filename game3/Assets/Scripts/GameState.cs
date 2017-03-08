using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	public BuildBoard board_builder;
	public GameObject pawn_unit;
	public static bool game_start;
	bool end_turn;
	public GameObject[] player1_units = new GameObject[64]; //change size later
	public GameObject[] player2_units = new GameObject[64]; //change size later
	public int p1_units;
	public int p2_units;
	int turn_phase;
	bool spawn;
	int player_turn = 0;

	private UnitScript selectedUnit = null;
	public GameObject unitPrefab;

	// Use this for initialization
	void Start () {
		game_start = false;
		player_turn = 1;
		end_turn = false;
		turn_phase = 1;
		p1_units = 0;
		p2_units = 0;
		spawn = false;
	}
	
	// Update is called once per frame
	void Update () {
//		if (game_start) {
//			
//			if (end_turn) {
//				end_turn = false;
//				turn_phase++;
//			}
//			if (turn_phase == 1) {
//				player_turn = 1;
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//
//				for (int i = 0; i < BuildBoard.num_boards; i++) {
//					Board current_board = BuildBoard.all_boards [i].GetComponent<Board>();
//					if(current_board.isSpawner && current_board.GetOwner() == player_turn && !current_board.IsOccupied())
//						Debug.Log ("Checking board: " + i.ToString() + " = " + current_board.IsSelected().ToString());
//					if (!spawn && current_board.isSpawner && current_board.GetOwner() == player_turn && !current_board.IsOccupied() && current_board.IsSelected()) {
//						Debug.Log ("Completed Unit Spawn for P1");
//						Vector3 spawn_position = new Vector3 (BuildBoard.all_boards [i].transform.position.x, BuildBoard.all_boards [i].transform.position.y + 5,
//							BuildBoard.all_boards [i].transform.position.z);
//						player1_units [p1_units] = Instantiate (pawn_unit, spawn_position, Quaternion.identity);
//						//current_board.SetOccupied (true);
//						current_board.SetSelected (false);
//						spawn = true;
//					}
//				}
//			} else if (turn_phase == 2) {
//				spawn = false;
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//			} else if (turn_phase == 3) {
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//			} else if (turn_phase == 4) {
//				player_turn = 2;
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//
//				for (int i = 0; i < BuildBoard.num_boards; i++) {
//					Board current_board = BuildBoard.all_boards [i].GetComponent<Board>();
//					if (!spawn && current_board.isSpawner && current_board.GetOwner() == player_turn && !current_board.IsOccupied() && current_board.IsSelected()) {
//						Debug.Log ("Completed Unit Spawn for P2");
//						Vector3 spawn_position = new Vector3 (BuildBoard.all_boards [i].transform.position.x, BuildBoard.all_boards [i].transform.position.y + 5,
//							BuildBoard.all_boards [i].transform.position.z);
//						player2_units [p2_units] = Instantiate (pawn_unit, spawn_position, Quaternion.identity);
//						//current_board.SetOccupied (true);
//						spawn = true;
//					}
//				}
//			} else if (turn_phase == 5) {
//				spawn = false;
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//			} else if (turn_phase == 6) {
//				//Debug.Log ("turn_phase = " + turn_phase.ToString());
//			}
//
//			if (turn_phase > 6)
//				turn_phase = 1;
//			
//		}
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			board_builder.enable_build ();
//			game_start = true;
//		}
//		if (Input.GetKeyDown (KeyCode.E)) {
//			end_turn = true;
//		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			SpawnNewUnit ();
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			OnLeftMouseDown ();
		}
		else if(Input.GetMouseButtonDown(1))
		{
			OnRightMouseDown ();
		}
	}

	private void SpawnNewUnit()
	{
		//pick some arbitrary tile, for the time being
		Board tile = FindObjectOfType<Board> ();

		if (tile.IsOccupied () || unitPrefab == null) {
			Debug.LogError ("could not spawn unit");
			return;
		}

		GameObject newUnit = Instantiate (unitPrefab);
		tile.Occupy (newUnit.GetComponent<UnitScript>());
	}

	private void OnLeftMouseDown() 
	{
		int unitsLayerMask = 1 << 9;
		RaycastHit hitInfo;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, unitsLayerMask)) {
			SelectUnit (hitInfo.transform.GetComponent<UnitScript> ());
		} else {
			DeselectUnit ();
		}
	}

	private void OnRightMouseDown()
	{
		if (selectedUnit == null)
			return;

		int tilesLayerMask = 1 << 8;
		RaycastHit hitInfo;

		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, tilesLayerMask))
		{
			Board dest = hitInfo.transform.GetComponent<Board> ();

			if(!dest.IsOccupied()) 
			{
				selectedUnit.currentTile.Vacate ();
				dest.Occupy (selectedUnit);
			}
			else
			{
				//resolve
			}
		}
	}

	public void SelectUnit(UnitScript unit)
	{
		if (selectedUnit != null)
			DeselectUnit ();

		unit.Select ();
		selectedUnit = unit;
	}

	public void DeselectUnit()
	{
		if (selectedUnit == null)
			return;

		selectedUnit.Deselect ();
		selectedUnit = null;
	}

	public void OccupyTileWithUnit(Board tile)
	{
		tile.Occupy (selectedUnit);
	}
}
