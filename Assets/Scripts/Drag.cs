using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	private Rigidbody rb;
	Transform cursor;
	
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		rb = GetComponent <Rigidbody>();
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		cursor = GameObject.FindGameObjectWithTag ("Cursor").transform;
	}
	
	void OnMouseDrag()
	{
		transform.parent = cursor;
	}

	void OnMouseUp() {
		transform.parent = null;
	}
}
