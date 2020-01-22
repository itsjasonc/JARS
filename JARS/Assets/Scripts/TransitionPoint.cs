using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour {
    public int index;
    public GameObject targetPoint;
    public float transitionSpeed; // The amount of time it will take to move to the next TransitionPoint
    private const float radius = 10;

    private void Awake() {
    }

    public Requirement getRequirement() {
        return (gameObject.GetComponent<Requirement>() != null) ? gameObject.GetComponent<Requirement>() : null;
    }

    public bool metRequirements() {
        Requirement requirement = getRequirement();
        if (requirement != null) {
            if (requirement.overrideRequirement) return true;
            if (!requirement.isTargetComplete()) return false;
            if (requirement.timeAllotted > 0) { // If we have a temporal requirement
                if (!requirement.allottedTimePassed()) {
                    return false; // We haven't met the temporal requirement
                }
            }
        }
        return true;
    }

    public RailTrigger[] getRailTriggers() {
        return gameObject.GetComponents<RailTrigger>();
    }

    public void ActivateZombies() {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in objects) {
            GameObject owner = Helper.findParentWithTag(collider.gameObject);
            if (owner != null && owner.activeSelf) {
                GameObject target = RandomPlayer();
                if (target != null)
                    owner.GetComponent<Zombie>().setTarget(RandomPlayer());
            }
        }
    }

    private GameObject RandomPlayer() {
        GameObject target = null;
        Transform[] ts = Camera.main.transform.Find("Players").GetComponentsInChildren<Transform>();
        List<GameObject> players = new List<GameObject>();
        foreach (Transform t in ts) {
            if (t.tag.Equals("Player"))
                players.Add(t.gameObject);
        }
        target = players[Helper.randomInt(0, players.Count - 1)].gameObject;
        return target;
    }
}
