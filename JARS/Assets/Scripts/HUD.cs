using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] Sprite heartSprite;
    [SerializeField] Sprite bulletSprite;
    [SerializeField] GameObject boss;
	private List<Player> players;
	private const string MESSAGE_RELOAD = "RELOAD!";
	private const string MESSAGE_START = "PRESS START TO PLAY";
	private const string MESSAGE_HOLD = "HOLD YOUR FIRE!";
	private GameObject playerHUDs;
    private GameObject bossHealth;
    private Vector2 resolution;
    Vector2 reticleSize;

    void Awake () {
        resolution = new Vector2(Screen.width, Screen.height);
        players = new List<Player>();
        GameObject[] unsorted = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] sorted = unsorted.OrderBy(go => go.name).ToArray();
        foreach (GameObject obj in sorted) {
            players.Add(obj.GetComponent<Player>());
        }
        playerHUDs = gameObject.transform.Find("Player HUDs").gameObject;
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
            reticleSize = gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject.GetComponent<RectTransform>().sizeDelta;
            GameObject playerHUD = playerHUDs.transform.Find("Player " + (playerIndex + 1) + " HUD").gameObject;
            GameObject UI = playerHUD.transform.Find("UI").gameObject;
            Text message = playerHUD.transform.Find("Message Holder").Find("Message").gameObject.GetComponent<Text>();
            int health = players[playerIndex].getHealth();
            for (int i = 0; i < health; i++) {
                CreateHeart(playerHUD);
            }
            if (players[playerIndex].hasWeapon()) {
                int ammoCount = players[playerIndex].getCurrentWeapon().getAmmoCount();
                for (int i = 0; i < ammoCount; i++) {
                    CreateBullet(playerHUD);
                }
            }
            if (!players[playerIndex].isActive() && Time.timeScale == 1) {
                gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject.SetActive(false);
                if (UI.activeSelf) UI.SetActive(false);
                if (players[playerIndex].getHealth() == 0) {
                    if (!message.text.Equals(MESSAGE_START)) message.text = MESSAGE_START;
                } else {
                    if (!message.text.Equals(MESSAGE_HOLD)) message.text = MESSAGE_HOLD;
                }
            } else {
                gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject.SetActive(true);
                if (players[playerIndex].isInMenu()) {
                    if (UI.activeSelf) UI.SetActive(false);
                    message.text = "";
                }
            }
        }
        bossHealth = transform.Find("Boss Health").gameObject;
        if (boss != null) {
            bossHealth.GetComponent<Slider>().maxValue = boss.GetComponent<Boss1>().getMaxHealth();
            bossHealth.GetComponent<Slider>().value = boss.GetComponent<Boss1>().getHealth();
        }
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (resolution.x != Screen.width || resolution.y != Screen.height) {
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }
	
	void OnGUI () {
        if (boss != null) {
             bossHealth.GetComponent<Slider>().value = boss.GetComponent<Boss1>().getHealth();
        }
        for (int playerIndex = 0; playerIndex < players.Count; playerIndex++) {
			GameObject playerHUD = playerHUDs.transform.Find("Player " + (playerIndex + 1) + " HUD").gameObject;
			GameObject UI = playerHUD.transform.Find("UI").gameObject;
			Text message = playerHUD.transform.Find("Message Holder").Find("Message").gameObject.GetComponent<Text>();
			GameObject reticle = gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject;
            
            if (players[playerIndex].getHealth() == 0) {
                if (gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject.activeSelf) {
                    gameObject.transform.Find("Player " + (playerIndex + 1) + " Reticle").gameObject.SetActive(false);
                }
                if (UI.activeSelf) UI.SetActive(false);
                if (!message.text.Equals(MESSAGE_START)) message.text = MESSAGE_START;
            } else {
                float scale = GetComponent<RectTransform>().rect.width / resolution.x;
                float posX = players[playerIndex].getCoordinates().x * scale - (GetComponent<RectTransform>().rect.width / 2) - (reticleSize.x / 2);
                float posY = players[playerIndex].getCoordinates().y * scale - (GetComponent<RectTransform>().rect.height / 2) - (reticleSize.y / 2);

                reticle.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0);
                if (!players[playerIndex].isActive()) {
                    if (!message.text.Equals(MESSAGE_HOLD)) message.text = MESSAGE_HOLD;
                } else {
                    if (players[playerIndex].isInMenu()) {
                        if (UI.activeSelf) UI.SetActive(false);
                        message.text = "";
                    } else {
                        if (!UI.activeSelf) UI.SetActive(true);
                        if (!reticle.activeSelf) {
                            reticle.SetActive(true);
                        }
                        GameObject healthGO = UI.transform.Find("Health").gameObject;
                        GameObject ammoGO = UI.transform.Find("Ammo").gameObject;
                        GameObject scoreGO = UI.transform.Find("Score").gameObject;
                        scoreGO.GetComponent<Text>().text = players[playerIndex].getScore().ToString("00000000");
                        int healthDiff = healthGO.transform.childCount - players[playerIndex].getHealth();
                        if (healthDiff > 0) {
                            StartCoroutine(FadeTo(playerIndex, 0.0f, 0.5f));
                            for (int i = 0; i < healthDiff; i++) {
                                Destroy(healthGO.transform.GetChild(healthGO.transform.childCount - 1).gameObject);
                            }
                        } else if (healthDiff < 0) {
                            healthDiff = -healthDiff;
                            for (int i = 0; i < healthDiff; i++) {
                                CreateHeart(playerHUD);
                            }
                        }
                        int ammoCount = players[playerIndex].getCurrentWeapon().getAmmoCount();
                        int ammoDiff = ammoGO.transform.childCount - ammoCount;
                        if (ammoDiff > 0) {
                            for (int i = 0; i < ammoDiff; i++) {
                                Destroy(ammoGO.transform.GetChild(ammoGO.transform.childCount - 1).gameObject);
                            }
                        } else if (ammoDiff < 0) {
                            ammoDiff = -ammoDiff;
                            for (int i = 0; i < ammoDiff; i++) {
                                CreateBullet(playerHUD);
                            }
                        }
                        if (ammoCount == 0) {
                            if (!message.text.Equals(MESSAGE_RELOAD)) message.text = MESSAGE_RELOAD;
                        } else {
                            if (message.text.Length > 0) message.text = "";
                        }
                    }
                }
            }
        }
    }

    IEnumerator FadeTo(int player, float aValue, float aTime) {
        GameObject damage = playerHUDs.transform.Find("Player " + (player + 1) + " HUD").Find("Damage").gameObject;
        Image img = damage.GetComponent<Image>();
        Color tempColor = img.color;
        tempColor.a = aTime;
        img.color = tempColor;
        float alpha = img.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime) {
            float newA = Mathf.Lerp(alpha, aValue, t);
            tempColor.a = newA;
            img.color = tempColor;
            yield return null;
        }
    }

    Image CreateImage(GameObject go, Sprite sprite) {
        Image img = go.AddComponent<Image>();
        img.sprite = sprite;
        return img;
    }

    void CreateHeart(GameObject player) {
        CreateImage(Helper.CreateGameObject("Heart", player.transform.Find("UI").transform.Find("Health"), new Vector2(80, 100)), heartSprite);
    }

    void CreateBullet(GameObject player) {
        CreateImage(Helper.CreateGameObject("Bullet", player.transform.Find("UI").transform.Find("Ammo"), new Vector2(50, 100)), bulletSprite);
    }
}
