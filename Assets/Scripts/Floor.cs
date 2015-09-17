using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Block") {
			GameState.ChangeState(GameState.State.Placing);
		}
	}
}
