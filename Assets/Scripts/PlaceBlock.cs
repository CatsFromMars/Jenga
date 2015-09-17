using UnityEngine;
using System.Collections;

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
		Instantiate(block, this.transform.position, this.transform.rotation);
		GameState.ChangeState(GameState.State.Taking);
	}
}
