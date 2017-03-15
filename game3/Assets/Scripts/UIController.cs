using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public RectTransform player1;
	public RectTransform player2;

	private Text playerText;
	private GameState gameStateHandler;

	// Use this for initialization
	void Start () {
		Text[] texts = GetComponentsInChildren<Text> ();

		if(texts[0].gameObject.name == "PlayerText")
			playerText = texts [0];
		else
			playerText = texts [1];

		gameStateHandler = FindObjectOfType<GameState> ();

		ButtonClicked ();
	} 
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyDown (KeyCode.E))
//			ButtonClicked ();
	}

	public void ButtonClicked()
	{
		if (gameStateHandler.gameOver)
			return;

		gameStateHandler.AdvanceTurn ();
		UpdatePosition ();
	}

	private void UpdatePosition()
	{
		if (gameStateHandler.currentPlayer == 2) {
			transform.position = player2.position;
			playerText.text = "Player 2's Turn";
		} else {
			transform.position = player1.position;
			playerText.text = "Player 1's Turn";
		}
	}
}
