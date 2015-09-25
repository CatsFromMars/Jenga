using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {
	private static GameObject currentBlock;

	private float maxVelocity = 5f;
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
		float oldY = rb.velocity.y;
		Vector3 newVelocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
		newVelocity.y = oldY; // don't clamp falling speed
		rb.velocity = newVelocity;
	}

	void OnMouseOver() {
		if (IsDisabled()) {
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
		if (IsDisabled()) {
			return;
		}

		cursor = GameObject.FindGameObjectWithTag("Cursor").transform;
		initialDragPos = cursor.position - transform.position;
		cam.camActive = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		currentBlock = gameObject;
	}

	void OnMouseDrag()
	{
		if (IsDisabled()) {
			return;
		}
		Debug.Log(transform.position.y);

		// Scale max velocity based on distance of cursor
		Vector3 origin = initialDragPos + transform.position;
		maxVelocity = (cursor.position - origin).magnitude * 0.1f;
		Vector3 force = Vector3.Project(cursor.position - origin, transform.forward);
		force *= Mathf.Log(force.magnitude + 1f, 2f) * 100f;
		rb.AddForce(force);
	}

	void OnMouseUp() {
		cam.camActive = true;
		rb.constraints = RigidbodyConstraints.None;
		maxVelocity = 100f;

		r.material.SetColor("_Color", Color.white);
		currentBlock = null;
	}

	bool IsDisabled() {
		if (GameState.CurrentState != GameState.State.Taking) {
			return true;
		}
		if (GameState.maxHeight > 0 && GameState.maxHeight - transform.position.y <= 0.002f) {
			return true;
		}
		return false;
	}
}
