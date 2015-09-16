using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum State {
        Taking,   // removing a piece from the Jenga tower
        Placing,  // placing a piece on the Jenga tower
        GameOver, // everything collapsed
    };

    private static GameState gameState;

    private State currentState;
    public GameObject finger;
    public bool enableCheats;

    public static State CurrentState {
        get {
            return gameState.currentState;
        }
    }

    public static GameObject Finger {
        get {
            return gameState.finger;
        }
    }

    // Handle state change logic
    public static void ChangeState(State newState) {
        switch (newState) {
            case State.Taking:
                Finger.SetActive(true);
                break;

            case State.Placing:
                Finger.SetActive(false);
                break;

            case State.GameOver:
                Finger.SetActive(false);
                // TODO(jason): slow motion, play music
                break;
        }

        gameState.currentState = newState;
    }

    void Awake() {
        gameState = this;
    }

    // Use this for initialization
    void Start () {
        gameState.finger = finger;
    }

    // Update is called once per frame
    void Update () {
        if (!enableCheats) {
            return;
        }

        if (Input.GetKeyDown("1")) {
            Debug.Log("switching to state: taking");
            ChangeState(State.Taking);
        } else if (Input.GetKeyDown("2")) {
            Debug.Log("switching to state: placing");
            ChangeState(State.Placing);
        } else if (Input.GetKeyDown("3")) {
            Debug.Log("switching to state: gameover");
            ChangeState(State.GameOver);
        }
    }

}
