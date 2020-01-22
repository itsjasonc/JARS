using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    private const int MAX_SCORES = 10;
    public List<Score> scores;
    public int pOneSens = 5;
    public int pTwoSens = 5;

    public GameData() {
        scores = new List<Score>();
        for (int i = 0; i < MAX_SCORES; i++) {
            scores.Add(new Score());
        }
    }
};

[Serializable]
public class Score {
    public int playerNumber;
    public string name;
    public int score;
    public int shotsFired;
    public int shotsHit;
    public int shotsWeakSpot;
    public int kills;

    public Score() {
        name = "John Doe";
        score = 0;
        shotsFired = 0;
        shotsHit = 0;
        shotsWeakSpot = 0;
        kills = 0;
    }

    public Score(int playerNumber, int score, int shotsFired, int shotsHit, int shotsWeakSpot, int kills) {
        this.playerNumber = playerNumber;
        this.score = score;
        this.shotsFired = shotsFired;
        this.shotsHit = shotsHit;
        this.shotsWeakSpot = shotsWeakSpot;
        this.kills = kills;
    }
};

public class Storage {
    private static Storage INSTANCE = null;
    private GameData gameData;
    private const string FILENAME = "jars.dat";

    private Storage() { gameData = new GameData(); }

    public static Storage getInstance() {
        if (INSTANCE == null) {
            INSTANCE = new Storage();
            INSTANCE.Load();
        }
        return INSTANCE;
    }

    public void Load() {
        string saveData = PlayerPrefs.GetString("Data");
        if (saveData.Length > 0) {
            gameData = JsonUtility.FromJson<GameData>(saveData);
        } else {
            gameData = new GameData();
        }
    }

    public void Save() {
        string saveData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("Data", saveData);
        PlayerPrefs.Save();
    }

    public void Update(Score score) {
        for (int i = 0; i < gameData.scores.Count; i++) {
            if (score.score > gameData.scores[i].score) {
                gameData.scores.Insert(i, score);
                gameData.scores.RemoveAt(gameData.scores.Count - 1);
                break;
            }
        }
        Save();
    }

    public bool IsHighscore(int score) {
        for (int i = 0; i < gameData.scores.Count; i++) {
            if (score > gameData.scores[i].score) {
                return true;
            }
        }
        return false;
    }

    public GameData GetGameData() {
        return gameData;
    }
}