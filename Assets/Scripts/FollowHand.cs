using UnityEngine;
using System.Collections;

public class FollowHand : MonoBehaviour {
	public Transform handController; //Assign me
	private GameObject hand;
	
	// Update is called once per frame
	void Update () {
		if (hand == null) {
			hand = GameObject.FindGameObjectWithTag ("Palm");
		} 
		else
			transform.position = hand.transform.position;
	}
}
