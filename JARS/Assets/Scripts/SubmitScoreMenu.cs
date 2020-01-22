using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class SubmitScoreMenu : MenuController {
    [SerializeField] Text playerOneText;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] GameObject board;
    public static List<Score> scores = new List<Score>();
    private string playerOneName = "";
    private const int MAX_NAME_LENGTH = 12;

    private void Awake() {
    }

    // Use this for initialization
    void Start() {
        Init();
        for (int i = 0; i < scores.Count; i++) {
            players[scores[i].playerNumber].setActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
        bool allWaiting = players.All(player => bool.Equals(false, player.isActive()));
        if (allWaiting) {
            for (int i = 0; i < scores.Count; i++) {
                if (scores[i].playerNumber == 0) {
                    scores[i].name = playerOneName;
                }
                Storage.getInstance().Update(scores[i]);
            }
            SceneManager.LoadScene("MenuScene");
        } else {
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                if (players[playerIndex].isActive()) {
                    Vector2 coords = players[playerIndex].getCoordinates();
                    if (Input.GetButtonUp("Player " + playerIndex + " Interact")) {
                        if (players[playerIndex].interact()) {
                            RaycastHit hit;
                            Vector3 position = new Vector3(coords.x, coords.y, 0);
                            Ray ray = Camera.main.ScreenPointToRay(position);
#if (UNITY_EDITOR)
                            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5);
#endif
                            if (Physics.Raycast(ray, out hit)) {
                                if (hit.collider.transform.parent.gameObject.tag.Equals("NameButton")) {
                                    string name = hit.collider.transform.parent.gameObject.name;
                                    if (name.Equals("Del")) {
                                        if (playerIndex == 0) {
                                            if (playerOneName.Length > 0) {
                                                playerOneName = playerOneName.Substring(0, playerOneName.Length - 1);
                                            }
                                        }
                                    } else if (name.Equals("Done")) {
                                        if (playerIndex == 0) {
                                            if (playerOneName.Length > 0)
                                                players[playerIndex].setActive(false);
                                        }
                                    } else if (name.Equals("_")) {
                                        if (playerIndex == 0) {
                                            if (playerOneName.Length < MAX_NAME_LENGTH) {
                                                playerOneName += " ";
                                            }
                                        }
                                    } else {
                                        for (char c = '0'; c <= '9'; c++) {
                                            if (c.ToString().Equals(name)) {
                                                if (playerIndex == 0) {
                                                    if (playerOneName.Length < MAX_NAME_LENGTH) {
                                                        playerOneName += name;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        for (char c = 'A'; c <= 'Z'; c++) {
                                            if (c.ToString().Equals(name)) {
                                                if (playerIndex == 0) {
                                                    if (playerOneName.Length < MAX_NAME_LENGTH) {
                                                        playerOneName += name;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        UpdateFrame();
    }

    private void OnGUI() {
        if (!playerOneText.text.Equals(playerOneName)) { 
            playerOneText.text = nameWithUnderscore(playerOneName);
        }
    }

    private string nameWithUnderscore(string name) {
        string temp = name;
        if (name.Length < MAX_NAME_LENGTH) {
            int diff = MAX_NAME_LENGTH - name.Length;
            for (int i = 0; i < diff; i++) {
                temp += "_";
            }
        }
        return temp;
    }
}
