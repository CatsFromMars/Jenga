using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {
	private static GameObject currentBlock;

	public float maxVelocity = 5f;

	private Rigidbody rb;
	private Transform cursor;
	private MeshRenderer r;
	private CameraControl cam;
	private Vector3 initialDragPos;

	void Awake() {
		r = GetComponent<MeshRenderer>();
		rb = GetComponent<Rigidbody>();
		cam = Camera.main.GetComponent<CameraControl>();
	}

	void Update() {
		// Limit maximum velocity of block
		if (rb.velocity.magnitude > maxVelocity) {
			rb.AddForce(-30f * rb.velocity.normalized);
		}
	}

	void OnMouseEnter() {
		if (GameState.CurrentState != GameState.State.Taking) {
			return;
		}
		if (currentBlock != null && currentBlock != gameObject) {
			return;
		}
		r.material.SetColor("_Color", new Color(0.8f, 0.8f, 1f, 1f)); // light blue
	}

	void OnMouseExit() {
		if (currentBlock == gameObject) {
			return;
		}
		r.material.SetColor("_Color", Color.white);
	}

	void OnMouseDown() {
		if (GameState.CurrentState != GameState.State.Taking) {
			return;
		}

		cursor = GameObject.FindGameObjectWithTag("Cursor").transform;
		initialDragPos = cursor.position;
		cam.camActive = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		currentBlock = gameObject;
	}

	void OnMouseDrag()
	{
		if (GameState.CurrentState != GameState.State.Taking) {
			return;
		}

		Vector3 force = Vector3.Project(cursor.position - initialDragPos, transform.forward);
		force = Vector3.ClampMagnitude(force * 5f, 10f);
		rb.AddForce(force);
	}

	void OnMouseUp() {
		cam.camActive = true;
		rb.constraints = RigidbodyConstraints.None;

		r.material.SetColor("_Color", Color.white);
		currentBlock = null;
	}
}
