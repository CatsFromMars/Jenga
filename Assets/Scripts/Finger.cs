using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	private Rigidbody rb;
	
	void Awake()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		rb = GetComponent <Rigidbody>();
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void Update()
	{
		Debug.Log ("MOVING!");
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
		transform.position = curPosition;
		
	}
}
