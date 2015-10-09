using UnityEngine;
using System.Collections;
using Leap;

public class Pincher : MonoBehaviour {
	public static bool IsPinching() {
        Frame currentFrame = GameState.HandController.GetFrame();
        if (currentFrame.Hands.IsEmpty) {
            return false;
        }

        Hand hand = currentFrame.Hands[0];
		Debug.Log (hand.PinchStrength);
        return hand.PinchStrength > 0.3f;
    }
}
