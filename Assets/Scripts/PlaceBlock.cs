using UnityEngine;
using System.Collections;

public class PlaceBlock : MonoBehaviour {
	public GameObject block;

	//private Vector3 screenPoint;
	//private Vector3 offset;
	//private Rigidbody rb;
	//Transform cursor;
	MeshRenderer r;
	private CameraControl cam;

	void Awake() {
		r = GetComponent<MeshRenderer>();
		cam = Camera.main.GetComponent<CameraControl>();
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
		gameObject.SetActive(false);
		Instantiate(block, this.transform.position, this.transform.rotation);
		//cam.camActive = true;
		//transform.parent = null;
	}
}
