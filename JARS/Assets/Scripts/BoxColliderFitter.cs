using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderFitter : MonoBehaviour {
    BoxCollider bc;

    private void Awake() {
    }

    // Use this for initialization
    void Start () {
        bc = GetComponent<BoxCollider>();
        StartCoroutine(St());
    }

    private IEnumerator St() {
        RectTransform parent = transform.parent.gameObject.GetComponent<RectTransform>();
        yield return null;
        yield return new WaitForEndOfFrame();
        bc.size = new Vector3(parent.sizeDelta.x, parent.sizeDelta.y, 1);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
