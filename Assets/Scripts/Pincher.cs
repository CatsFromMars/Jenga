using UnityEngine;
using System.Collections;
using Leap;

public class Pincher : MonoBehaviour {
	public bool pinching = false;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Thumb" || other.gameObject.tag == "Index") {
			Debug.Log ("PINCH");
			pinching = true;
		}
	}
}
