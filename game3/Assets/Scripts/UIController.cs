using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Transform player1;
	public Transform player2;

	private Text playerText;
	private Text buttonText;
	private GameState gameStateHandler;

	// Use this for initialization
	void Start () {
		Text[] texts = GetComponentsInChildren<Text> ();

		if(texts[0].gameObject.name == "PlayerText")
		{
			playerText = texts [0];
			buttonText = texts [1];
		}
		else
		{
			buttonText = texts [0];
			playerText = texts [1];
		}

		gameStateHandler = FindObjectOfType<GameState> ();

		ButtonClicked ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ButtonClicked()
	{
		if (gameStateHandler.gameOver)
			return;

		gameStateHandler.AdvanceTurn ();
		SwapPosition ();
	}

	private void SwapPosition()
	{
		if (transform.position == player1.position) {
			transform.position = player2.position;
			playerText.text = "Player 2";
		} else {
			transform.position = player1.position;
			playerText.text = "Player 1";
		}
	}
}
