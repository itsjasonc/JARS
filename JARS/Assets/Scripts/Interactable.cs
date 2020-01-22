using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour {
	public enum Type {
		Damage
	}

    protected bool active = true;
	
	/**
	* This function is called when it is interacted with by an object
	*
	* @param objs The first index is always the GameObject that interacted with it
	*/
	public virtual void onInteract(object[] objs) {
    }
    public bool isActive() { return active; }
    public void setActive(bool active) { this.active = active; }
}
