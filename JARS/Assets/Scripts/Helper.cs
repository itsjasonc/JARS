using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Helper {
    public static System.Random random = new System.Random();

    public static GameObject CreateGameObject(string name, Transform transform, Vector2 size) {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
		go.transform.localPosition = new Vector3(0, 0, 0);
		go.transform.localRotation = Quaternion.identity;
        RectTransform goTrans = go.AddComponent<RectTransform>();
        goTrans.sizeDelta = size;
        goTrans.localScale = new Vector3(1, 1, 1);
        return go;
    }

    public static void CreateText(ref GameObject go, Font font, string str) {
        Text text = go.AddComponent<Text>();
        text.text = str;
        text.font = font;
        text.fontSize = 16;
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;
    }

    public static Vector3 Lerp(Vector3 A, Vector3 B, float t) {
        return A * t + B * (1f - t);
    }

    public static void SpawnZombie(ref GameObject obj, Vector3 position, Quaternion rotation) {
        Object.Instantiate(obj, position, rotation);
    }
	
	public static IEnumerator DisplayCaption(GameObject caption, string message) {
		caption.GetComponent<Text>().text = message;
		yield return new WaitForSeconds(3.0f);
		caption.GetComponent<Text>().text = "";
	}

    public static GameObject findParentWithTagNeedle(string tag, GameObject current) {
        var parent = current.transform.parent;
        while (parent != null) {
            if (parent.tag == tag) {
                return parent.gameObject as GameObject;
            }
            parent = parent.transform.parent;
        }
        return null;
    }

    public static GameObject findParentWithTag(GameObject current) {
        var parent = current.transform.parent;
        while (parent != null) {
            if (parent.tag != "Untagged") { 
                return parent.gameObject as GameObject;
            }
            parent = parent.transform.parent;
        }
        return null;
    }

    public static int randomInt(int start, int end) {
        return random.Next(start, end);
    }
}
