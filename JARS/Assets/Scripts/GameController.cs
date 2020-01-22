using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    [SerializeField] GameObject menu;
	[SerializeField] GameObject gunshotEffect;
    [SerializeField] GameObject bloodEffect;
    [SerializeField] GameObject boss;
    private List<Player> players;
    private Vector2 resolution;
    private float gameEndTime;

    void Awake() {
        Cursor.visible = false;
        SubmitScoreMenu.scores.Clear();
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
            players[playerIndex].setCoordinates(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        players[0].setSensitivity(Storage.getInstance().GetGameData().pOneSens);
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (resolution.x != Screen.width || resolution.y != Screen.height) {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                players[playerIndex].setCoordinates(new Vector2(resolution.x / 2, resolution.y / 2));
            }
        }
        bool allDead = players.All(player => int.Equals(0, player.getHealth()));
        if (allDead || (boss != null && boss.GetComponent<Boss1>().getHealth() <= 0)) {
            Camera.main.GetComponent<RailMover>().isCompleted = true;
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                players[playerIndex].setActive(false);
            }
            if (gameEndTime == 0) {
                gameEndTime = Time.time;
            }
            if ((Time.time - gameEndTime) > 3f) {
                ScoreScene.scores = players;
                SceneManager.LoadScene("Score Scene");
            }
        } else if (allDead && boss == null) {
            Camera.main.GetComponent<RailMover>().isCompleted = true;
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                players[playerIndex].setActive(false);
            }
            if (gameEndTime == 0) {
                gameEndTime = Time.time;
            }
            if ((Time.time - gameEndTime) > 3f) {
                ScoreScene.scores = players;
                SceneManager.LoadScene("Score Scene");
            }
        } else {
            // Check for Input
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                Vector2 coords = players[playerIndex].getCoordinates();
                if (Input.GetButtonUp("Pause")) {
                    PauseButton();
                }
                if (players[playerIndex].getHealth() > 0) {
                    if (Input.GetButtonUp("Player " + playerIndex + " Interact")) {
                        if (players[playerIndex].getCurrentWeapon().getFiringMode() == Weapon.FIRING_MODE.SINGLE) {
                            if (players[playerIndex].interact()) {
                                if (players[playerIndex].isActive() || (!players[playerIndex].isActive() && players[playerIndex].isInMenu())) {
                                    RaycastHit hit;
                                    Vector3 position = new Vector3(coords.x, coords.y, 0);
                                    Ray ray = Camera.main.ScreenPointToRay(position);
#if (UNITY_EDITOR)
                                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5);
#endif
                                    if (Physics.Raycast(ray, out hit)) {
                                        if (!players[playerIndex].isInMenu()) {
                                            players[playerIndex].incrementShotsHit();
                                            GameObject owner = Helper.findParentWithTag(hit.collider.gameObject);
                                            if (owner != null) {
                                                bool weakSpot = false;
                                                switch (hit.collider.gameObject.tag) {
                                                    default:
                                                    case "Untagged":
                                                        break;
                                                    case "WeakSpot":
                                                        weakSpot = true;
                                                        break;
                                                }
                                                MonoBehaviour[] scripts = owner.GetComponents<MonoBehaviour>();
                                                object[] data = new object[6];
                                                data[0] = players[playerIndex];
                                                data[1] = Interactable.Type.Damage;
                                                data[2] = players[playerIndex].getCurrentWeapon().getDamage();
                                                data[3] = weakSpot;
                                                bool hitInteractable = false;
                                                for (int i = 0; i < scripts.Length; i++) {
                                                    if (scripts[i].GetType().IsSubclassOf(typeof(Interactable))) {
                                                        owner.SendMessage("onInteract", data);
                                                        hitInteractable = true;
                                                        break;
                                                    }
                                                }
                                                GameObject effect;
                                                if (hitInteractable) {
                                                    effect = Instantiate(bloodEffect, hit.point, hit.transform.rotation);
                                                } else {
                                                    effect = Instantiate(gunshotEffect, hit.point, hit.transform.rotation);
                                                }
                                                effect.transform.SetParent(hit.collider.gameObject.transform);
                                            }
                                        } else {
                                            switch (hit.collider.gameObject.transform.parent.gameObject.name) {
                                                case "Quit Button":
                                                    Time.timeScale = 1;
                                                    SceneManager.LoadScene("MenuScene");
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!players[playerIndex].isInMenu() && players[playerIndex].isActive()) {
                        // If the player is reloading
                        if (Input.GetButtonUp("Player " + playerIndex + " Reload")) {
                            players[playerIndex].reload();
                        }
                    }
                }
                players[playerIndex].setCoordinates(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
        }
    }

    void PauseButton() {
        menu.SetActive(!menu.activeSelf);
        for (int i = 0; i < players.Count; i++) {
            players[i].setInMenu(!players[i].isInMenu());
        }
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource AS in audioSources) {
            if (AS.isPlaying) {
                AS.Pause();
            } else {
                AS.UnPause();
            }
        }
    }
}
