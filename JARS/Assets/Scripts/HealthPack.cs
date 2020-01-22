using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Interactable {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
	
	public override void onInteract(object[] objs) {
		switch (((Interactable)objs[0]).name) {
			case "Player One":
			case "Player Two":
				((Interactable)objs[0]).GetComponent<Player>().increaseHealth(1);
				DestroyObject(this.gameObject);
				break;
		}
	}
}
