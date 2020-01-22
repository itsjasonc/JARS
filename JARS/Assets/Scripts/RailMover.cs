using System.Collections;
using UnityEngine;

public class RailMover : MonoBehaviour {
	public Rail rail; // The rail we are on
	public PlayMode mode;

    private const float DEFAULT_SPEED = 10f;
	
	// private int currentSeg; // The current segment that we are on
    private int currentNodeIndex; // The current node we are on
	private float transitionPosition; // The point on a segment that we are on
	private float transitionQuaternion; // The rotation we should be looking at
	public bool isCompleted; // Whether we have completed
    private float lastEnemyTime;
    private float ENEMY_KILLED_DELAY = 1f;
	
	private void Update() {
		if (!rail)
			return;
		
		if (rail.nodes.Count == 0) {
			isCompleted = true;
			return;
		}
		
		if (!isCompleted) {
			Move();
		}
	}

    private void Move() {
        if (Time.timeScale == 0) return;
        TransitionPoint tp = rail.nodes[currentNodeIndex].gameObject.GetComponent<TransitionPoint>();
        RailTrigger[] rts = tp.getRailTriggers();

        if (rts.Length > 0) {
			foreach(RailTrigger rt in rts) {
				if (!rt.activated) {
					if (rt.onEnter) {
						rt.Activate();
					}
				}
			}
        }

        // Does this TP have requirements before we can move?
        if (!tp.metRequirements()) return; // If we haven't met the requirements, do not move
        if (Camera.main.GetComponent<CameraCollider>().EnemiesNearby()) {
            lastEnemyTime = Time.time;
            return; // Don't do anything if enemies nearby
        } else {
            if ((Time.time - lastEnemyTime) < ENEMY_KILLED_DELAY) {
                return;
            }
        }
		
        int targetIndex = (tp.targetPoint != null) ? tp.targetPoint.gameObject.GetComponent<TransitionPoint>().index : currentNodeIndex + 1;
		
		if (targetIndex >= rail.nodes.Count) { // We are on the last node
			// We've reached the end
			isCompleted = true;
			return;
		}
		
        Vector3 targetPosition = rail.nodes[targetIndex].position;
        Quaternion targetRotation = rail.nodes[targetIndex].rotation;

        // We have met all requirements, we can move
        // The magnitude of the next TP and the current one
        float mP = (targetPosition - rail.nodes[currentNodeIndex].position).magnitude;
        // The point on the segment that we are on, based on the speed of the TP
        float sP = (Time.deltaTime * 1 / mP) * tp.transitionSpeed;
        transitionPosition += sP;

        float mQ = Quaternion.Angle(targetRotation, rail.nodes[currentNodeIndex].rotation);
        float sQ = (Time.deltaTime * 1 / mQ) * (tp.transitionSpeed * 10);
        transitionQuaternion += sQ;
		
        if ((transitionPosition >= 1 && transitionQuaternion >= 1) || (transitionPosition == Mathf.Infinity && transitionQuaternion == Mathf.Infinity)) { // If we are moving forwards and reached our destination
			if (rts.Length > 0) {
				foreach(RailTrigger rt in rts) {
					if (!rt.activated) {
						if (!rt.onEnter) {
							rt.Activate();
						}
					}
				}
			}
            transitionPosition = 0; // set the point on the line back to the beginning
            transitionQuaternion = 0;
			if (tp.targetPoint == null)
				currentNodeIndex++; // Just go to the next segment
			else {
				currentNodeIndex = tp.targetPoint.gameObject.GetComponent<TransitionPoint>().index;
			}
            if (currentNodeIndex == rail.nodes.Count) { // If we are on the last node
				if (tp.targetPoint == null) {
					isCompleted = true;
					return;
				} else {
					currentNodeIndex = tp.targetPoint.gameObject.GetComponent<TransitionPoint>().index;
				}
            }
        }

        // Move us
        transform.position = rail.PositionOnRail(currentNodeIndex, targetIndex, transitionPosition, mode);
        transform.rotation = rail.Orientation(currentNodeIndex, targetIndex, transitionQuaternion);
    }
}