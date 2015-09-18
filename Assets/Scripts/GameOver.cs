using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	Text txt;
	// Use this for initialization
	void Start () {
		txt = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.CurrentState == GameState.State.GameOver) {
			txt.text = "GAME OVER";
			//Slow Down Time
			Time.timeScale = 0.2f;
		}
	}
}
