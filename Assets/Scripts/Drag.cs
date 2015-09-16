using UnityEngine;
using System.Collections;

public class Drag : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	private Rigidbody rb;
	Transform cursor;
	MeshRenderer r;
	private CameraControl cam;

	void Awake() {
		r = GetComponent<MeshRenderer>();
		cam = Camera.main.GetComponent<CameraControl>();
	}

	void OnMouseEnter() {
		r.material.SetColor("_Color", Color.red);
	}

	void OnMouseExit() {
		r.material.SetColor("_Color", Color.white);
	}
	
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		rb = GetComponent <Rigidbody>();
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		cursor = GameObject.FindGameObjectWithTag ("Cursor").transform;
	}
	
	void OnMouseDrag()
	{
		//transform.parent = cursor;
		cam.camActive = false;
		rb.AddForce((cursor.position - transform.position)*10);
	}

	void OnMouseUp() {
		cam.camActive = true;
		//transform.parent = null;
	
	}
}
