using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MenuController {
    [SerializeField] Text pOneSens;
    [SerializeField] GameObject scoreboard;
    private GameObject canvases;
    private GameObject mainMenuCanvas;
    private GameObject highScoreCanvas;
    private GameObject optionsCanvas;

	// Use this for initialization
	void Start () {
        Init();

        canvases = GameObject.Find("Canvases");
        mainMenuCanvas = canvases.transform.Find("Main Menu Canvas").gameObject;
        highScoreCanvas = canvases.transform.Find("High Score Canvas").gameObject;
        optionsCanvas = canvases.transform.Find("Options Canvas").gameObject;

        Storage.getInstance().Load();
        GameData data = Storage.getInstance().GetGameData();
        Transform[] ts = scoreboard.GetComponentsInChildren<Transform>();
        List<GameObject> placements = new List<GameObject>();
        foreach (Transform t in ts) {
            if (t.gameObject.name.Equals("Placement")) {
                placements.Add(t.gameObject);
            }
        }
        for (int i = 0; i < data.scores.Count; i++) {
            placements[i].transform.Find("Name").gameObject.GetComponent<Text>().text = data.scores[i].name;
            placements[i].transform.Find("Score").gameObject.GetComponent<Text>().text = data.scores[i].score.ToString("00000000");
        }
    }
	
	// Update is called once per frame
	void Update () {
        bool optionChanged = false;
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
#if (UNITY_EDITOR)
                            Debug.Log(hit.collider.gameObject.transform.parent.gameObject.name);
#endif
                            switch (hit.collider.gameObject.transform.parent.gameObject.name) {
                                case "Play Button":
                                    SceneManager.LoadScene("Level");
                                    break;
                                case "High Score Button":
                                    DisplayHighScore();
                                    break;
                                case "Options Button":
                                    DisplayOptions();
                                    break;
                                case "Quit Button":
                                    Application.Quit();
                                    break;
                                case "Back Button":
                                    DisplayMainMenu();
                                    break;
                                case "P1SensDown":
                                    players[0].decrementSensitivity();
                                    Storage.getInstance().GetGameData().pOneSens = players[0].getSensitivity();
                                    optionChanged = true;
                                    break;
                                case "P1SensUp":
                                    players[0].incrementSensitivity();
                                    Storage.getInstance().GetGameData().pOneSens = players[0].getSensitivity();
                                    optionChanged = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }
        UpdateFrame();
        if (optionChanged) {
            Storage.getInstance().Save();
        }
    }

    void HideAllCanvases() {
        foreach(Transform t in canvases.transform) {
            if (t != this.gameObject.transform) {
                t.gameObject.SetActive(false);
            }
        }
    }

    void DisplayMainMenu() {
        HideAllCanvases();
        mainMenuCanvas.SetActive(true);
    }

    void DisplayHighScore() {
        HideAllCanvases();
        highScoreCanvas.SetActive(true);
    }

    void DisplayOptions() {
        HideAllCanvases();
        optionsCanvas.SetActive(true);
    }

    private void OnGUI() {
        if (!pOneSens.text.Equals(players[0].getSensitivity().ToString())) {
            pOneSens.text = players[0].getSensitivity().ToString();
        }
    }
}
