using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	private Rigidbody rb;
	private CameraControl cam;
	private int childrenCount;
	
	void Awake()
	{
		childrenCount = transform.childCount;
		cam = Camera.main.GetComponent<CameraControl>();
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		rb = GetComponent <Rigidbody>();
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void Update()
	{
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint);
		rb.MovePosition (curPosition);
	}
}
