using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	void Start () {
		
	}

	void Update () {
		
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}

	public void QuitApplication()
	{
		Application.Quit ();
	}
}
