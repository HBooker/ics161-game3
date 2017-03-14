using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {
	//int turn_phase = 3;
	bool hasSpawned = false;
	bool hasUpgraded = false;
	int currentPlayer = 2;

	int[] sacrifice = {0, 0};
	private UnitScript selectedUnit = null;
	public GameObject unitPrefab;

	public Material[] unitDefaultMaterials = new Material[2];
	public Material[] unitSelectedMaterials = new Material[2];
	public bool gameOver = false;


	void Start () {
	}

	void Update () {
		if (gameOver)
			return;

		if(selectedUnit != null)
		{
			if (Input.GetKeyDown (KeyCode.S))
				SacrificeSelectedUnit ();
			else if (Input.GetKeyDown (KeyCode.U))
				UpgradeUnit ();
			else if (Input.GetKeyDown (KeyCode.I))
				InvincibleUnit ();
		}

		if (Input.GetMouseButtonDown (0)) 
		{
			OnLeftMouseDown ();
		}
		else if( Input.GetMouseButtonDown(1))
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
		int unit_lvl = unit.powerLevel;
		sacrifice[currentPlayer - 1] += unit_lvl;

		if(currentPlayer == 1)
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
		RemoveTileHighlights ();
	}

	private void UpgradeUnit()
	{
		if (sacrifice [currentPlayer - 1] == 0)
			return;

		selectedUnit.AddPower (1);
		sacrifice[currentPlayer - 1] -= 1;
		hasUpgraded = true;
		 
		if(currentPlayer == 1)
		{
			GameObject.FindGameObjectWithTag ("p1sac").GetComponent<Text>().text = "P1 Sacrifice: " + sacrifice[0];
		}
		else
		{
			GameObject.FindGameObjectWithTag ("p2sac").GetComponent<Text>().text = "P2 Sacrifice: " + sacrifice[1];
		}

		HighlightSelectedUnitMoves ();
	}
	private void InvincibleUnit()
	{
		if (sacrifice [currentPlayer - 1] == 0)
			return;

		selectedUnit.SetInvincible (true);
		sacrifice[currentPlayer - 1] -= 1;

		if(currentPlayer == 1)
		{
			GameObject.FindGameObjectWithTag ("p1sac").GetComponent<Text>().text = "P1 Sacrifice: " + sacrifice[0];
		}
		else
		{
			GameObject.FindGameObjectWithTag ("p2sac").GetComponent<Text>().text = "P2 Sacrifice: " + sacrifice[1];
		}

		HighlightSelectedUnitMoves ();
	}

	private UnitScript SpawnNewUnit(Board tile)
	{
		if(unitPrefab == null)
		{
			Debug.LogError ("Spawn error: no unit prefab");
			return null;
		}

		if (tile.IsOccupied ()) {
			print ("Unit not spawned: tile is occupied");
			return null;
		}

		UnitScript newUnit = Instantiate (unitPrefab).GetComponent<UnitScript>();
		newUnit.playerOwner = currentPlayer;
		newUnit.SetMaterials (unitDefaultMaterials [currentPlayer - 1], unitSelectedMaterials [currentPlayer - 1]);
		tile.Occupy (newUnit);
		hasSpawned = true;
		return newUnit;
	}

	private void OnLeftMouseDown() 
	{
		int unitsLayerMask = 1 << 9;
		RaycastHit hitInfo;
		UnitScript unit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, unitsLayerMask)) {
			unit = hitInfo.transform.GetComponent<UnitScript> ();
			if (unit.playerOwner == currentPlayer)
				SelectUnit (hitInfo.transform.GetComponent<UnitScript> ());
		} else
			DeselectUnit ();
	}



	private void OnRightMouseDown()
	{
		if (selectedUnit == null)
			return;

		int tilesLayerMask = 1 << 8;
		RaycastHit hitInfo;
		Board dest;

		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, tilesLayerMask))
		{
			dest = hitInfo.transform.GetComponent<Board> ();

			if(selectedUnit.CanMoveToTile(dest)) 
			{
				//selectedUnit.MoveToTile(dest);

				if(!dest.IsOccupied())
				{
					selectedUnit.currentTile.Vacate ();
					dest.Occupy (selectedUnit);
					selectedUnit.SetMoved (true);
					DeselectUnit ();
				}
				else
				{
					if(dest.occupyingUnit.playerOwner != currentPlayer)
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

		if(!unit.GetMoved())
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

	public void AdvanceTurn()
	{
		int previousPlayer = currentPlayer;
		hasSpawned = false;
		hasUpgraded = false;

		if (currentPlayer == 1)
			currentPlayer = 2;
		else
			currentPlayer = 1;

		if(GameIsOver())
		{
			gameOver = true;
			Text gameOverText = GameObject.FindGameObjectWithTag ("Finish").GetComponent<Text>();
			gameOverText.text = "PLAYER " + previousPlayer + " HAS WON";
			gameOverText.enabled = true;
		}

		GameObject[] units = GameObject.FindGameObjectsWithTag ("unit");
		foreach (GameObject unit in units)
			if (unit.GetComponent<UnitScript> ().playerOwner == currentPlayer) {
				unit.GetComponent<UnitScript> ().SetMoved (false);
				unit.GetComponent<UnitScript> ().SetInvincible (false);
			}

		GameObject[] tiles = GameObject.FindGameObjectsWithTag("tile");
		foreach(GameObject t in tiles)
		{
			Board tile = t.GetComponent<Board> ();
			if (tile.player_owner == currentPlayer)
				SpawnNewUnit (tile);
		}


	}
	 
	private bool GameIsOver()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("tile");
		int tilesCaptured = 0;

		foreach(GameObject tileObj in tiles)
		{
			Board tile = tileObj.GetComponent<Board> ();

			if(tile.player_owner == currentPlayer && tile.IsOccupied() && tile.occupyingUnit.playerOwner != tile.player_owner)
			{
				++tilesCaptured;
			}
		}

		return (tilesCaptured == 2);
	}
}
