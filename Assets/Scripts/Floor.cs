using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
	private int resetCounter = 0;
	private int resetWaitTime = 10;
	public int blockCounter = 0;
	private AudioSource a;

	void Awake() {
		a = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Block") {
			blockCounter++;

			//Switch States
			if(blockCounter > 1)
				GameState.ChangeState(GameState.State.GameOver);
			else if (GameState.CurrentState!=GameState.State.GameOver)
				GameState.ChangeState(GameState.State.Placing);

			Destroy(other.gameObject);
			if(!a.isPlaying) a.Play();
		}
	}

	void Update() {
		if (resetCounter >= resetWaitTime) {
			resetCounter = 0;
			blockCounter = 0;
		}
		else
			resetCounter++;
	}
}
