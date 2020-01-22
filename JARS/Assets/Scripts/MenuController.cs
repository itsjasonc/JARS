using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    protected List<Player> players;
    protected Vector2 resolution;

    void Awake() {
    }

    // Use this for initialization
    void Start() {
        Cursor.visible = false;
    }
    
    protected void Init() {
        resolution = new Vector2(Screen.width, Screen.height);
        Player.MAX_RETICLE_SIZE.x = resolution.x;
        Player.MAX_RETICLE_SIZE.y = resolution.y;
        Player.MIN_RETICLE_SIZE.x = 0;
        Player.MIN_RETICLE_SIZE.y = 0;
        players = new List<Player>();
        GameObject[] unsorted = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] sorted = unsorted.OrderBy(go => go.name).ToArray();
        foreach (GameObject obj in sorted) {
            players.Add(obj.GetComponent<Player>());
        }
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
            players[playerIndex].setInMenu(true);
            players[playerIndex].setCoordinates(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        players[0].setSensitivity(Storage.getInstance().GetGameData().pOneSens);
    }

    protected void UpdateFrame() {
        if (resolution.x != Screen.width || resolution.y != Screen.height) {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                players[playerIndex].setCoordinates(new Vector2(resolution.x / 2, resolution.y / 2));
            }
        }
        // Check for Input
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
            if (players[playerIndex].isActive()) {
                players[playerIndex].setCoordinates(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
        }
    }
}
