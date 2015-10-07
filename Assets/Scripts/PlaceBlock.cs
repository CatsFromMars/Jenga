using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceBlock : MonoBehaviour {
	public GameObject block;
	private static GameObject currentBlock;
	MeshRenderer r;

	void Awake() {
		r = GetComponent<MeshRenderer>();
	}

	/* Mouse controls */
	void OnMouseEnter() {
		if (GameState.IsLeapActive) return;
		OnHover();
	}
	void OnMouseExit() {
		if (GameState.IsLeapActive) return;
		OnHoverExit();
	}
	void OnMouseUp() {
		if (GameState.IsLeapActive) return;
		Place();
	}


	void OnHover() {
		Color color = r.material.color;
		color.a = 0.5f;
		r.material.color = color;

		if (currentBlock != null && currentBlock != gameObject) {
			currentBlock.GetComponent<PlaceBlock>().OnHoverExit();
		}
		currentBlock = gameObject;
	}

	void OnHoverExit() {
		Color color = r.material.color;
		color.a = 0.2f;
		r.material.color = color;
		currentBlock = null;
	}

	void Place() {
		if (currentBlock != gameObject) {
			return;
		}

		// Add block to Tower
		GameObject newBlock = Instantiate(block, this.transform.position, this.transform.rotation) as GameObject;
		newBlock.transform.parent = GameState.Tower.transform;
		GameState.ChangeState(GameState.State.Taking);
		GameState.numPlaced++;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Index") {
			OnHover();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Index") {
			OnHoverExit();
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Index" && Pincher.IsPinching()) {
			Place();
		}
	}
}
