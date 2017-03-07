using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateBoardInEditor : MonoBehaviour
{
	public int boardWidth = 8;
	public int boardHeight = 8;
	public float tileWidth = 1.0f;
	public GameObject tilePrefab;

	private const string GAMEBOARD_TAG = "gameboard";

	void Update()
	{
		enabled = !UnityEditor.EditorApplication.isPlaying;
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(10, 10, 150, 100), "Generate Board (" + boardWidth + "x" + boardHeight + ")"))
		{
			GenerateNewBoard (boardWidth, boardHeight);
		}
	}

	private void GenerateNewBoard(int width, int height)
	{
		int numBoards = GameObject.FindGameObjectsWithTag (GAMEBOARD_TAG).Length;
		float halfWidth = tileWidth / 2;

		//can still run into duplicate names if multiple boards are created, some deleted, then more created
		GameObject boardObject = new GameObject("GameBoard" + numBoards++);


		boardObject.tag = GAMEBOARD_TAG;

		for(int i = 0; i < width; ++i)
		{
			for(int j = 0; j < height; ++j)
			{
				Vector3 tilePosition = new Vector3 (i * tileWidth + halfWidth, 0, j * tileWidth + halfWidth);
				//Instantiate (tilePrefab, tilePosition, Quaternion.identity, boardObject.transform);
				GameObject tile = (GameObject) UnityEditor.PrefabUtility.InstantiatePrefab (tilePrefab);
				tile.transform.position = tilePosition;
				tile.transform.rotation = Quaternion.identity;
				tile.transform.SetParent (boardObject.transform);
			}
		}
	}
}
