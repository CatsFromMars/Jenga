using UnityEngine;
using System.Collections;

public class FollowHand1 : MonoBehaviour {

	GameObject hand;

	void Awake() {
		hand = GameObject.FindGameObjectWithTag("Palm");
	}
	
	// Update is called once per frame
	void Update () {
		if(hand == null) hand = GameObject.FindGameObjectWithTag("Palm");
		else transform.position = hand.transform.position;
	}
}
