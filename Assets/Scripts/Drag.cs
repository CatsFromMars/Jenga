using UnityEngine;
using System.Collections;
using Leap;

public class Drag : MonoBehaviour {
	private static GameObject currentBlock;
	private bool dragging = false;

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

		if (dragging) {
			if (IsPinching()) {
				OnDrag();
			} else {
				OnRelease();
			}
		}
	}

	/* Mouse controls */
	/*void OnMouseDown() {
		OnStartDrag();
	}
	void OnMouseDrag() {
		OnDrag();
	}
	void OnMouseUp() {
		OnRelease();
	}
	void OnMouseOver() {
		OnHover();
	}
	void OnMouseExit() {
		OnHoverExit();
	}*/


	/* Called when hovering over or pointing at block */
	void OnHover() {
		if (IsDisabled() || dragging) {
			return;
		}
		if (currentBlock != null && currentBlock != gameObject) {
			Drag lastDrag = currentBlock.GetComponent<Drag>();
			if (lastDrag.dragging) {
				return;
			}
			currentBlock = gameObject;
			lastDrag.OnHoverExit();
		}
		r.material.SetColor("_Color", new Color(0.8f, 0.8f, 1f, 1f)); // light blue
		currentBlock = gameObject;
	}

	/* Begin dragging a block, whether via mouse or finger */
	void OnStartDrag() {
		if (IsDisabled() || currentBlock != gameObject) {
			return;
		}

		cursor = GameObject.FindGameObjectWithTag("Palm").transform;
		initialDragPos = cursor.position;
		cam.camActive = false;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		dragging = true;
	}

	/* Called every frame that block is dragged */
	void OnDrag()
	{
		if (IsDisabled()) {
			return;
		}

		cursor = GameObject.FindGameObjectWithTag("Palm").transform;

		// Scale max velocity based on distance of cursor
		Vector3 origin = initialDragPos;
		maxVelocity = (cursor.position - origin).magnitude * 0.1f;
		Vector3 force = Vector3.Project(cursor.position - origin, transform.forward);
		force *= Mathf.Log(force.magnitude + 1f, 2f) * 20f;
		rb.AddForce(force);
	}

	/* Called when mouse or finger leaves block */
	void OnHoverExit() {
		if (currentBlock == gameObject) {
			return;
		}
		r.material.SetColor("_Color", Color.white);
	}

	/* Called when block is released */
	void OnRelease() {
		cam.camActive = true;
		rb.constraints = RigidbodyConstraints.None;
		maxVelocity = 100f;

		r.material.SetColor("_Color", Color.white);
		currentBlock = null;
		dragging = false;
	}

	bool IsDisabled() {
		// disable blocks during placement stage, or if at top of tower
		if (GameState.CurrentState != GameState.State.Taking) {
			return true;
		}
		if (GameState.maxHeight > 0 && GameState.maxHeight - transform.position.y <= 0.002f) {
			return true;
		}
		return false;
	}

	bool IsPinching() {
		Frame currentFrame = GameState.HandController.GetFrame();
		if (currentFrame.Hands.IsEmpty) {
			return false;
		}

		Hand hand = currentFrame.Hands[0];
		return hand.PinchStrength > 0.25f;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Index") {
			OnHover();
		}
	}

	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag == "Index") {
			OnHoverExit();
		}
	}

	void OnCollisionStay(Collision collision) {
		// Start dragging if hand is currently pinching
		if (!dragging && collision.gameObject.tag == "Index" && IsPinching()) {
			OnStartDrag();
			//if(cursor.transform.FindChild("Cube") == null) this.transform.parent = cursor;
		}
	}
}
