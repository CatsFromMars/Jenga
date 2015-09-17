using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum State {
        Taking,   // removing a piece from the Jenga tower
        Placing,  // placing a piece on the Jenga tower
        GameOver, // everything collapsed
    };

    private static GameState gameState;
    private static CameraControl cam;

    private State currentState;
    //public GameObject finger;
    public GameObject[] placeLocations;
    public bool enableCheats;

    public static State CurrentState {
        get {
            return gameState.currentState;
        }
    }

    public static GameObject[] PlaceLocations {
        get {
            return gameState.placeLocations;
        }
    }

    // Handle state change logic
    public static void ChangeState(State newState) {
        switch (newState) {
            case State.Taking:
                Debug.Log("switching to state: taking");
                for (int i = 0; i < 3; i++) {
                    PlaceLocations[i].SetActive(false);
                }
                break;

            case State.Placing:
                Debug.Log("switching to state: placing");
                for (int i = 0; i < 3; i++) {
                    PlaceLocations[i].SetActive(true);
                }

                cam.ScrollToTarget(new Vector3(1f, 9.5f, 1.5f));
                break;

            case State.GameOver:
                Debug.Log("switching to state: gameover");
                for (int i = 0; i < 3; i++) {
                    PlaceLocations[i].SetActive(false);
                }
                // TODO(jason): slow motion, play music
                break;
        }

        gameState.currentState = newState;
    }

    // Use this for initialization
    void Awake() {
        gameState = this;
        cam = Camera.main.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update () {
        if (!enableCheats) {
            return;
        }

        if (Input.GetKeyDown("1")) {
            ChangeState(State.Taking);
        } else if (Input.GetKeyDown("2")) {
            ChangeState(State.Placing);
        } else if (Input.GetKeyDown("3")) {
            ChangeState(State.GameOver);
        }
    }

}
