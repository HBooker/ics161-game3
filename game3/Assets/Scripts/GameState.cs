using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {
	public int currentPlayer = 2;
	public int maxRounds = 20;
	int[] sacrifice = {0, 6};
	private UnitController selectedUnit = null;
	public GameObject unitPrefab;

	public AudioSource bgm;
	public AudioSource[] soundeffects;
	public Material[] unitDefaultMaterials = new Material[2];
	public Material[] unitSelectedMaterials = new Material[2];
	public bool gameOver = false;
	private int roundsRemaining;

	void Start () {
		//bgm.enabled = false;
		roundsRemaining = maxRounds + 1;
	}

	void Update () {
		for (int i = 0; i < soundeffects.Length; ++i) {
			soundeffects[i].volume = bgm.volume;
		}
		if (Input.GetKeyDown (KeyCode.Z))
			bgm.volume -= 0.1f;
		if (Input.GetKeyDown (KeyCode.X))
			bgm.volume += 0.1f;

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

	private UnitController[] GetAllUnits()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("unit");
		List<UnitController> units = new List<UnitController>();

		foreach(GameObject obj in objs)
			units.Add (obj.GetComponent<UnitController> ());

		return units.ToArray ();
	}

	private UnitController[] GetAllPlayerUnits(int player)
	{
		UnitController[] allUnits = GetAllUnits ();
		List<UnitController> units = new List<UnitController>();

		foreach(UnitController unit in allUnits)
		{
			if (unit.playerOwner == player)
				units.Add (unit);
		}

		return units.ToArray ();
	}

	private TileController[] GetAllTiles()
	{
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("tile");
		TileController[] tiles = new TileController[objs.Length];

		for (int i = 0; i < tiles.Length; ++i)
			tiles [i] = objs [i].GetComponent<TileController> ();

		return tiles;
	}

	private TileController[] GetAllPlayerSpawners(int player)
	{
		TileController[] tiles = GetAllTiles ();
		List<TileController> playerSpanwers = new List<TileController> ();

		foreach(TileController tile in tiles)
		{
			if(tile.player_owner == player)
				playerSpanwers.Add (tile);
		}

		return playerSpanwers.ToArray ();
	}

	private void DestroyUnit(UnitController unit)
	{
		unit.currentTile.Vacate ();
		Destroy (unit.gameObject);
	}

	private void DestroySelectedUnit()
	{
		DestroyUnit (selectedUnit);
		selectedUnit = null;
	}

	private void UpdatePlayerSacrificeCounter(int player)
	{
		string objTag = "p" + currentPlayer + "sac";
		string sacrificeText = "Player " + currentPlayer + " Scrap: " + sacrifice[currentPlayer - 1];
		GameObject.FindGameObjectWithTag (objTag).GetComponent<Text>().text = sacrificeText;
	}

	private void AddPlayerSacrificePoints(int player, int points)
	{
		sacrifice [player - 1] += points;
		UpdatePlayerSacrificeCounter (player);
	}

	private void SacrificeUnit(UnitController unit)
	{	
		AddPlayerSacrificePoints (currentPlayer, unit.powerLevel);
		DestroyUnit (unit);
	}

	private void SacrificeSelectedUnit()
	{
		UnitController unit = selectedUnit;
		DeselectUnit ();
		SacrificeUnit (unit);
	}

	private void UpgradeUnit()
	{
		if (sacrifice [currentPlayer - 1] == 0)
			return;

		selectedUnit.AddPower (1);
		sacrifice[currentPlayer - 1] -= 1;
		UpdatePlayerSacrificeCounter (currentPlayer);
		HighlightSelectedUnitMoves ();
	}
	private void InvincibleUnit()
	{
		if (sacrifice [currentPlayer - 1] == 0)
			return;

		selectedUnit.SetInvincible (true);
		sacrifice[currentPlayer - 1] -= 1;
		UpdatePlayerSacrificeCounter (currentPlayer);
		HighlightSelectedUnitMoves ();
	}

	private UnitController SpawnNewUnit(TileController tile)
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

		UnitController newUnit = Instantiate (unitPrefab).GetComponent<UnitController>();
		newUnit.playerOwner = currentPlayer;
		newUnit.FlipSprite ();
		newUnit.SetMaterials (unitDefaultMaterials [currentPlayer - 1], unitSelectedMaterials [currentPlayer - 1]);
		if (currentPlayer == 2) {
			//
		}
		tile.Occupy (newUnit);
		return newUnit;
	}

	private void OnLeftMouseDown() 
	{
		int unitsLayerMask = 1 << 9;
		RaycastHit hitInfo;
		UnitController unit;

		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, Mathf.Infinity, unitsLayerMask)) {
			unit = hitInfo.transform.GetComponent<UnitController> ();
			if (unit.playerOwner == currentPlayer ) //&& !unit.GetMoved()
				SelectUnit (hitInfo.transform.GetComponent<UnitController> ());
		} else
			DeselectUnit ();
	}

	private void EndGame(int playerWinner)
	{
		gameOver = true;
		Text gameOverText = GameObject.FindGameObjectWithTag ("Finish").GetComponent<Text>();
		gameOverText.text = "PLAYER " + playerWinner + " HAS WON";
		gameOverText.enabled = true;
	}

	private void OnRightMouseDown()
	{
		if (selectedUnit == null)
			return;

		int tilesLayerMask = 1 << 8;
		RaycastHit hitInfo;
		TileController dest;

		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, tilesLayerMask))
		{
			dest = hitInfo.transform.GetComponent<TileController> ();

			if(selectedUnit.CanMoveToTile(dest)) 
			{
				if(!dest.IsOccupied())
				{
					soundeffects [3].Play ();
					MoveUnitToTile (selectedUnit, dest);
					DeselectUnit ();
				}
				else //if(dest.occupyingUnit.playerOwner != currentPlayer)
				{
					ResolveCombat (selectedUnit, dest.occupyingUnit);
				}

				if(PlayerSpawnersCaptured(currentPlayer))
				{
					EndGame (currentPlayer);
				}
				else if (!CurrentPlayerHasMovesAvailable())
				{
					//auto end turn OR enable some UI element telling player to end the turn
				}
			}
		}
	}

	private void MoveUnitToTile(UnitController unit, TileController tile)
	{
		unit.currentTile.Vacate ();
		tile.Occupy (unit);
		unit.SetMoved (true);
	}

	private void ResolveCombat(UnitController attacker, UnitController defender)
	{
		int powerDifference = attacker.powerLevel - defender.powerLevel;
		attacker.SetAttacking ();
		soundeffects[0].Play ();
		if (powerDifference >= 0) {
			soundeffects [2].Play ();
			AddPlayerSacrificePoints (currentPlayer, 1);
			if (powerDifference > 0) //should attacker occupy defender's tile?
				StartCoroutine (kill_tank_and_move (defender, attacker));
			else
				StartCoroutine (kill_tank (defender));
			
		} else {
			defender.AddPower (-attacker.powerLevel);
			soundeffects [1].Play ();
			defender.SetHit ();
		}

		attacker.SetMoved (true);
		DeselectUnit ();
	}

	private bool CurrentPlayerHasMovesAvailable()
	{
		UnitController[] units = GetAllPlayerUnits (currentPlayer);

		foreach(UnitController unit in units)
			if (!unit.GetMoved ())
				return true;
		
		return false;
	}

	private void HighlightSelectedUnitMoves()
	{
		TileController[] tiles = GetAllTiles ();

		foreach(TileController tile in tiles)
			tile.SetHighlight (selectedUnit.CanMoveToTile (tile));
	}

	public void SelectUnit(UnitController unit)
	{
		if (selectedUnit != null)
			DeselectUnit ();

		unit.Select ();
		selectedUnit = unit;

		if(!unit.GetMoved())
			HighlightSelectedUnitMoves ();
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
		TileController[] tiles = GetAllTiles ();

		foreach(TileController tile in tiles)
			tile.SetHighlight (false);
	}

	private void UpdateRounds()
	{
		GameObject.FindGameObjectWithTag ("rounds").GetComponent<Text>().text = "Rounds Remaining: " + roundsRemaining;
	}

	public void AdvanceTurn()
	{
		UpdatePlayerSacrificeCounter (currentPlayer);
		currentPlayer = currentPlayer % 2 + 1;

//		if (currentPlayer == 1) {
//			roundsRemaining--;
//			UpdateRounds ();
//		}
//
//		if(roundsRemaining == 0)
//		{
//			EndGame (2);
//			return;
//		}

		UnitController[] units = GetAllUnits ();
		foreach (UnitController unit in units)
		{
			if (unit.playerOwner == currentPlayer) 
			{
				unit.SetMoved (false);
				unit.SetInvincible (false);
			}
			else
			{
				unit.SetMoved (true);
			}
		}

		TileController[] tiles = GetAllPlayerSpawners (currentPlayer);
		foreach(TileController tile in tiles)
			SpawnNewUnit (tile);
	}

	private bool PlayerSpawnersCaptured(int player)
	{
		TileController[] tiles = GetAllPlayerSpawners(player);

		foreach(TileController tile in tiles)
			if(!tile.IsOccupied() || tile.occupyingUnit.playerOwner == player)
				return false;

		return true;
	}
	IEnumerator kill_tank (UnitController unit) {
		unit.SetDead();
		yield return new WaitForSeconds(1.3f);
		DestroyUnit (unit);
	}

	IEnumerator kill_tank_and_move (UnitController defender, UnitController attacker) {
		TileController dest = defender.currentTile;
		defender.SetDead();
		defender.SetInvincible (true);
		yield return new WaitForSeconds(1.2f);
		DestroyUnit (defender);
		MoveUnitToTile(attacker, dest);
	}

}
 