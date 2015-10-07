using UnityEngine;
using System.Collections;
using Leap;

[RequireComponent (typeof(Rigidbody))]
public class Drag : MonoBehaviour {
	public Color highlightColor = new Color(0.8f, 0.8f, 1f, 1f); // light blue
	private Color oldColor;

	private static GameObject currentBlock;
	private bool dragging = false;

	private float maxVelocity = 5f;
	private Rigidbody rb;
	private Transform cursor;
	private MeshRenderer r;
	private CameraControl cam;
	private Vector3 initialDragPos;
	private SoundEffects soundEffects;

	void Awake() {
		r = GetComponent<MeshRenderer>();
		rb = GetComponent<Rigidbody>();
		cam = Camera.main.GetComponent<CameraControl>();
		oldColor = r.material.color;
		soundEffects = GetComponent<SoundEffects>();
	}

	void Update() {
		// Limit maximum velocity of block
		float oldY = rb.velocity.y;
		Vector3 newVelocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
		newVelocity.y = oldY; // don't clamp falling speed
		rb.velocity = newVelocity;

		if (dragging) {
			if (Pincher.IsPinching()) {
				OnDrag();
			} else {
				OnRelease();
			}
		}
	}

	/* Mouse controls */
	void OnMouseDown() {
		if (GameState.IsLeapActive) return;
		OnStartDrag();
	}
	void OnMouseDrag() {
		if (GameState.IsLeapActive) return;
		OnDrag();
	}
	void OnMouseUp() {
		if (GameState.IsLeapActive) return;
		OnRelease();
	}
	void OnMouseOver() {
		if (GameState.IsLeapActive) return;
		OnHover();
	}
	void OnMouseExit() {
		if (GameState.IsLeapActive) return;
		OnHoverExit();
	}


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
		r.material.SetColor("_Color", highlightColor);
		currentBlock = gameObject;
	}

	/* Begin dragging a block, whether via mouse or finger */
	void OnStartDrag() {
		if (IsDisabled() || currentBlock != gameObject) {
			return;
		}

		cursor = GetCursorTransform();
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

		cursor = GetCursorTransform();

		// Scale max velocity based on distance of cursor
		Vector3 origin = initialDragPos;
		maxVelocity = (cursor.position - origin).magnitude * 0.1f;
		Vector3 force = Vector3.Project(cursor.position - origin, transform.forward);
		force *= Mathf.Log(force.magnitude + 1f, 2f) * 20f;

		if (!soundEffects.isPlaying) {
			soundEffects.PlayRandom();
		} else {
			Debug.Log(force.magnitude);
			soundEffects.SetVolume(force.magnitude / 30f);
		}
		rb.AddForce(force);
	}

	/* Called when mouse or finger leaves block */
	void OnHoverExit() {
		if (currentBlock == gameObject) {
			return;
		}
		r.material.SetColor("_Color", oldColor);
	}

	/* Called when block is released */
	void OnRelease() {
		cam.camActive = true;
		rb.constraints = RigidbodyConstraints.None;
		maxVelocity = 100f;

		r.material.SetColor("_Color", oldColor);
		currentBlock = null;
		dragging = false;

		if (soundEffects.isPlaying) {
			soundEffects.FadeOut();
		}
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
		if (!dragging && collision.gameObject.tag == "Index" && Pincher.IsPinching()) {
			OnStartDrag();
			//if(cursor.transform.FindChild("Cube") == null) this.transform.parent = cursor;
		}
	}

	Transform GetCursorTransform() {
		if (GameState.IsLeapActive) {
			return GameObject.FindGameObjectWithTag("Palm").transform;
		} else {
			return GameObject.FindGameObjectWithTag("Cursor").transform;
		}
	}
}
