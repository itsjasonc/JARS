using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RailTrigger : MonoBehaviour {
    public bool activated;
    public bool onEnter = true;
    public GameObject target;
    public bool hideTarget = false;
    public bool showTarget = false;
    public bool destroyTarget = false;
    public bool activatePlayer = false;
    public bool deactivatePlayer = false;
    public bool activateZombies = false;
    private GameObject HUD;
    public string caption;
    private GameObject player;

    private void Awake() {
        activated = false;
        HUD = GameObject.Find("HUD").gameObject;
        player = Camera.main.transform.Find("Players").Find("Player One").gameObject;
    }

    private void Start() {
    }

    public void Activate() {
        activated = true;
        if (target != null) {
            if (target.GetComponent<AudioSource>() != null) {
                target.GetComponent<AudioSource>().Play();
            } else {
                if (hideTarget) {
                    target.SetActive(false);
                } else if (showTarget) {
                    target.SetActive(true);
                } else if (destroyTarget) {
                    Destroy(target);
                } else if (activatePlayer) {
                    target.SendMessage("setActive", true);
                } else if (deactivatePlayer) {
                    target.SendMessage("setActive", false);
                } else {
                    target.GetComponent<Boss1>().setTarget(player);
                }
            }
        } else {
            if (activateZombies) {
                gameObject.GetComponent<TransitionPoint>().ActivateZombies();
            }
        }
        if (caption.Length > 0) {
            GameObject hudCaption = HUD.transform.Find("Caption").gameObject;
            if (hudCaption != null)
                StartCoroutine(Helper.DisplayCaption(hudCaption, caption));
        }
    }
}
