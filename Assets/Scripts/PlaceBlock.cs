using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceBlock : MonoBehaviour {
	public GameObject block;
	MeshRenderer r;

	void Awake() {
		r = GetComponent<MeshRenderer>();
	}

	void OnMouseEnter() {
		Color color = r.material.color;
		color.a = 0.5f;
		r.material.color = color;
	}

	void OnMouseExit() {
		Color color = r.material.color;
		color.a = 0.2f;
		r.material.color = color;
	}

	void OnMouseDown() {
	}

	void OnMouseUp() {
		// Add block to Tower
		GameObject newBlock = Instantiate(block, this.transform.position, this.transform.rotation) as GameObject;
		newBlock.transform.parent = GameState.Tower.transform;
		GameState.ChangeState(GameState.State.Taking);
	}
}
