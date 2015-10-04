using UnityEngine;
using System.Collections;

public class FollowHand1 : MonoBehaviour {
	GameObject finger;

	void Awake() {
		finger = GameObject.FindGameObjectWithTag("Thumb");
	}
	
	// Update is called once per frame
	void Update () {
		if (finger == null) {
			finger = GameObject.FindGameObjectWithTag("Thumb");
		} else {
			transform.position = finger.transform.position;
		}
	}
}
