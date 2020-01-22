using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScene : MenuController {
    public static List<Player> scores;
    [SerializeField] Text p1ShotsFired;
    [SerializeField] Text p1ShotsHit;
    [SerializeField] Text p1WeakSpotsHit;
    [SerializeField] Text p1EnemiesKilled;
    [SerializeField] Text p1Accuracy;
    [SerializeField] Text p1Score;

    [SerializeField] Text p2ShotsFired;
    [SerializeField] Text p2ShotsHit;
    [SerializeField] Text p2WeakSpotsHit;
    [SerializeField] Text p2EnemiesKilled;
    [SerializeField] Text p2Accuracy;
    [SerializeField] Text p2Score;
    private GameObject canvas;

    private void Awake() {
        canvas = GameObject.Find("Canvas");
    }

    // Use this for initialization
    void Start () {
        Init();
        for (int i = 0; i < scores.Count; i++) {
            players[i].setShotsFired(scores[i].getShotsFired());
            players[i].setShotsHit(scores[i].getShotsHit());
            players[i].setShotsWeakSpot(scores[i].getShotsWeakSpot());
            players[i].setEnemiesKilled(scores[i].getEnemiesKilled());
            players[i].setScore(scores[i].getScore() + (scores[i].getShotsFired() * (scores[i].getShotsWeakSpot() * (int)(((float)players[i].getShotsHit() / (float)players[i].getShotsFired()) * 1000))) * scores[i].getEnemiesKilled());
        }
        players[0].setActive(true);

        if (players.Count > 0) {
            GameObject playersGo = canvas.transform.Find("Players").gameObject;
            for (int i = 0; i < players.Count; i++) {
                if (players[i].isActive() && players[i].getHealth() > 0) {
                    GameObject pgo = playersGo.transform.Find("p" + (i + 1)).gameObject;
                    GameObject stats = pgo.transform.Find("Stats").gameObject;
                    Text shotsFiredText = stats.transform.Find("Row").transform.Find("Shots Fired").GetComponent<Text>();
                    Text shotsHitText = stats.transform.Find("Row (1)").transform.Find("Shots Hit").GetComponent<Text>();
                    Text weakSpotsHitText = stats.transform.Find("Row (2)").transform.Find("Weak Spots Hit").GetComponent<Text>();
                    Text enemiesKilledText = stats.transform.Find("Row (3)").transform.Find("Enemies Killed").GetComponent<Text>();
                    Text accuracyText = stats.transform.Find("Row (4)").transform.Find("Accuracy").GetComponent<Text>();
                    Text bonusText = stats.transform.Find("Row (5)").transform.Find("Bonus").GetComponent<Text>();
                    Text scoreText = stats.transform.Find("Row (6)").transform.Find("Score").GetComponent<Text>();
                    float accuracy = ((((float)players[i].getShotsHit() / (float)players[i].getShotsFired()) * 100));
                    shotsFiredText.text = players[i].getShotsFired().ToString();
                    shotsHitText.text = players[i].getShotsHit().ToString();
                    weakSpotsHitText.text = players[i].getShotsWeakSpot().ToString();
                    enemiesKilledText.text = players[i].getEnemiesKilled().ToString();
                    accuracyText.text = accuracy.ToString("0.00") + " %";
                    bonusText.text = (scores[i].getShotsWeakSpot() * (int)(((float)players[i].getShotsHit() / (float)players[i].getShotsFired()) * 1000) * scores[i].getEnemiesKilled()).ToString();
                    scoreText.text = players[i].getScore().ToString();
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateFrame();
        bool allWaiting = players.All(player => bool.Equals(false, player.isActive()));
        if (allWaiting) {
            bool highscore = false;
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                int score = players[playerIndex].getScore();
                int shotsFired = players[playerIndex].getShotsFired();
                int shotsHit = players[playerIndex].getShotsHit();
                int shotsWeakSpot = players[playerIndex].getShotsWeakSpot();
                int kills = players[playerIndex].getEnemiesKilled();
                if (Storage.getInstance().IsHighscore(score)) {
                    SubmitScoreMenu.scores.Add(new Score(playerIndex, score, shotsFired, shotsHit, shotsWeakSpot, kills));
                    highscore = true;
                }
            }
            if (highscore) {
                SceneManager.LoadScene("Submit Score");
            } else {
                SceneManager.LoadScene("MenuScene");
            }
        } else {
            UpdateFrame();
            for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
                if (players[playerIndex].isActive()) {
                    if (Input.GetButtonUp("Player " + playerIndex + " Interact")) {
                        if (players[playerIndex].interact()) {
                            players[playerIndex].setActive(false);
                        }
                    }
                }
            }
        }
    }
}
