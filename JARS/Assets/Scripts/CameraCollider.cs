using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour {
    private bool enemiesNearby;
    private const float radius = 10;

    void Awake() {
        enemiesNearby = false;
    }

    public bool EnemiesNearby() {
        return enemiesNearby;
    }

    private void FixedUpdate() {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius);
        enemiesNearby = false;
        foreach (Collider collider in objects) {
            GameObject owner = Helper.findParentWithTag(collider.gameObject);
            if (owner != null && owner.tag.Equals("Zombie") && (owner.GetComponent<Zombie>().getAlive() && owner.GetComponent<Zombie>().getTarget() != null)) {
                enemiesNearby = true;
                break;
            }
        }
    }
}
