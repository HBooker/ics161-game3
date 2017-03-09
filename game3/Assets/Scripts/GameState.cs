using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {
	public BuildBoard board_builder;
	public GameObject pawn_unit;
	public static bool game_start;
	bool end_turn;
	public GameObject[] player1_units = new GameObject[64]; //change size later
	public GameObject[] player2_units = new GameObject[64]; //change size later
	public int p1_units;
	public int p2_units;
	int turn_phase = 3;
	bool hasSpawned = false;
	bool hasUpgraded = false;
	int player_turn = 2;

	int[] sacrifice = {0, 0};
	private UnitScript selectedUnit = null;
	public GameObject unitPrefab;

	public Material[] unitDefaultMaterials = new Material[2];
	public Material[] unitSelectedMaterials = new Material[2];
	public bool gameOver = false;

	// Use this for initialization
	void Start () {
		game_start = false;
		end_turn = false;
		p1_units = 0;
		p2_units = 0;
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

		if (gameOver)
			return;

		if(Input.GetKeyDown(KeyCode.Delete) && selectedUnit != null)
		{
			DestroySelectedUnit();
		}

		if(turn_phase == 2 && !hasUpgraded && selectedUnit != null)
		{
			if (Input.GetKeyDown (KeyCode.S))
				SacrificeSelectedUnit ();
			else if (Input.GetKeyDown (KeyCode.U))
				UpgradeUnit ();
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			OnLeftMouseDown ();
		}
		else if(turn_phase == 3 && Input.GetMouseButtonDown(1))
		{
			OnRightMouseDown ();
		}
	}

	private void DestroyUnit(UnitScript unit)
	{
		unit.currentTile.Vacate ();
		Destroy (unit.gameObject);
	}

	private void DestroySelectedUnit()
	{
		DestroyUnit (selectedUnit);
		selectedUnit = null;
	}

	private void SacrificeUnit(UnitScript unit)
	{
		++sacrifice[player_turn - 1];

		if(player_turn == 1)
		{
			GameObject.FindGameObjectWithTag ("p1sac").GetComponent<Text>().text = "P1 Sacrifice: " + sacrifice[0];
		}
		else
		{
			GameObject.FindGameObjectWithTag ("p2sac").GetComponent<Text>().text = "P2 Sacrifice: " + sacrifice[1];
		}

		DestroyUnit (unit);
	}

	private void SacrificeSelectedUnit()
	{
		SacrificeUnit (selectedUnit);
		selectedUnit = null;
	}

	private void UpgradeUnit()
	{
		if (sacrifice [player_turn - 1] == 0)
			return;

		selectedUnit.AddPower (sacrifice[player_turn - 1]);
		//selectedUnit.powerLevel += sacrifice[player_turn - 1];
		sacrifice[player_turn - 1] = 0;
		hasUpgraded = true;
		 
		if(player_turn == 1)
		{
			GameObject.FindGameObjectWithTag ("p1sac").GetComponent<Text>().text = "P1 Sacrifice: 0";
		}
		else
		{
			GameObject.FindGameObjectWithTag ("p2sac").GetComponent<Text>().text = "P2 Sacrifice: 0";
		}
	}

	private UnitScript SpawnNewUnit(Board tile)
	{
		

		if (tile.IsOccupied () || unitPrefab == null) {
			//Debug.LogError ("could not spawn unit");
			print("could not spawn new unit");
			return null;
		}

		UnitScript newUnit = Instantiate (unitPrefab).GetComponent<UnitScript>();
		newUnit.playerOwner = player_turn;
		newUnit.SetMaterials (unitDefaultMaterials [player_turn - 1], unitSelectedMaterials [player_turn - 1]);
		tile.Occupy (newUnit);
		hasSpawned = true;
		return newUnit;
	}

	private void OnLeftMouseDown() 
	{
		int unitsLayerMask = 1 << 9;
		int tilesLayerMask = 1 << 8;
		RaycastHit hitInfo;
		UnitScript unit;
		Board tile;

		switch(turn_phase)
		{
		case 1:
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, tilesLayerMask)) {
				tile = hitInfo.transform.GetComponent<Board> ();
				if(!hasSpawned && tile.isSpawner && tile.player_owner == player_turn)
				{
					SpawnNewUnit (tile);
				}
			}
			break;
		case 2:
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, unitsLayerMask)) {
				unit = hitInfo.transform.GetComponent<UnitScript> ();
				if (unit.playerOwner == player_turn)
					SelectUnit (unit);
			} else
				DeselectUnit ();
			break;
		case 3:
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, unitsLayerMask)) {
				unit = hitInfo.transform.GetComponent<UnitScript> ();
				if (unit.playerOwner == player_turn)
					SelectUnit (hitInfo.transform.GetComponent<UnitScript> ());
			} else
				DeselectUnit ();
			break;
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

			if(selectedUnit.CanMoveToTile(dest)) 
			{
				if(!dest.IsOccupied())
				{
					selectedUnit.currentTile.Vacate ();
					dest.Occupy (selectedUnit);
					selectedUnit.SetMoved (true);
					DeselectUnit ();
				}
				else
				{
					if(dest.occupyingUnit.playerOwner != player_turn)
					{
						SacrificeUnit (dest.occupyingUnit);
						selectedUnit.currentTile.Vacate ();
						dest.Occupy (selectedUnit);
						selectedUnit.SetMoved (true);
						DeselectUnit ();
					}
				}
			}
		}
	}

	private void HighlightSelectedUnitMoves()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");

		foreach(GameObject tileObj in tiles)
		{
			Board tile = tileObj.GetComponent<Board> ();
			tile.SetHighlight (selectedUnit.CanMoveToTile (tile));
		}
	}

	public void SelectUnit(UnitScript unit)
	{
		if (selectedUnit != null)
			DeselectUnit ();

		unit.Select ();
		selectedUnit = unit;

		if(turn_phase == 3 && !unit.GetMoved())
		{
			HighlightSelectedUnitMoves ();
		}
	}

	public void DeselectUnit()
	{
		if (selectedUnit == null)
			return;

		selectedUnit.Deselect ();
		selectedUnit = null;

		RemoveTileHighlights ();
	}

	private void RemoveTileHighlights()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");

		foreach(GameObject tileObj in tiles)
		{
			Board tile = tileObj.GetComponent<Board> ();
			tile.SetHighlight (false);
		}
	}

	public void OccupyTileWithUnit(Board tile)
	{
		tile.Occupy (selectedUnit);
	}

	public string AdvanceTurn()
	{
		++turn_phase;

		switch(turn_phase)
		{
		case 2:
			Turn2Setup ();
			return "Upgrade Phase";
		case 3:
			Turn3Setup ();
			return "Movement Phase";
		case 4:
			turn_phase = 1;
			Turn1Setup ();
			return "Spawn Phase";
		}

		return "PHASE ERROR";
	}

	public int GetTurnPhase()
	{
		return turn_phase;
	}

	private bool GameIsOver()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");
		int tilesCaptured = 0;

		foreach(GameObject tileObj in tiles)
		{
			Board tile = tileObj.GetComponent<Board> ();

			if(tile.player_owner == player_turn && tile.IsOccupied() && tile.occupyingUnit.playerOwner != tile.player_owner)
			{
				++tilesCaptured;
			}
		}

		return (tilesCaptured == 2);
	}

	private void Turn1Setup()
	{
		int previousPlayer = player_turn;
		hasSpawned = false;

		if (player_turn == 1)
			player_turn = 2;
		else
			player_turn = 1;

		if(GameIsOver())
		{
			gameOver = true;
			Text gameOverText = GameObject.FindGameObjectWithTag ("Finish").GetComponent<Text>();
			gameOverText.text = "PLAYER " + previousPlayer + " HAS WON";
			gameOverText.enabled = true;
		}
	}

	private void Turn2Setup()
	{
		
		hasUpgraded = false;
	}

	private void Turn3Setup()
	{
		GameObject[] units = GameObject.FindGameObjectsWithTag ("unit");
		foreach (GameObject unit in units)
			unit.GetComponent<UnitScript> ().SetMoved(false);
	}
}
