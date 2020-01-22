using System.Collections;
using UnityEngine;

public class Requirement : MonoBehaviour {
	public float timeAllotted; // If set, this amount of time must pass
	private float timeStarted;
	public GameObject actionTarget; // If set, this object must be dead
	public bool overrideRequirement = false;
	
	private void Awake() {
		timeStarted = 0;
	}
	
	private void Update() {
		
	}
	
	public bool allottedTimePassed() {
		if (timeStarted == 0) timeStarted = Time.time;
		if (Time.time - timeStarted < timeAllotted) {
			return false;
		}
		return true;
	}
	
	public bool isTargetComplete() {
		if (actionTarget != null) {
			if (actionTarget.GetComponent<BoxCollider>().enabled) {
				return false;
			}
		}
		return true;
	}
}
