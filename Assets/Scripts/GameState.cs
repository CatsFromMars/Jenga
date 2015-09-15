using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum State {
        Taking,   // removing a piece from the Jenga tower
        Placing,  // placing a piece on the Jenga tower
        GameOver, // everything collapsed
    };

    public static State currentState = State.Taking;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

}
