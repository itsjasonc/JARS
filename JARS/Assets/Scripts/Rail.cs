using System.Collections.Generic;
#if (UNITY_EDITOR)
using UnityEditor;
#endif
using UnityEngine;

public enum PlayMode {
	Linear,
	Catmull
}

[ExecuteInEditMode]
public class Rail : MonoBehaviour {
	public List<Transform> nodes;

    private void Start() {
        nodes.Clear();
		int i = 0;
        foreach(Transform child in transform) {
			if (child.gameObject.activeSelf) {
				child.gameObject.GetComponent<TransitionPoint>().index = i++;
				nodes.Add(child);
			}
        }
    }
    
	/**
	* Allows switching between a linear movement and a catmull movement for the rail
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	* @param mode The movement mode
	*/
	public Vector3 PositionOnRail(int seg, float ratio, PlayMode mode) {
		switch (mode) {
			default:
			case PlayMode.Linear:
				return LinearPosition(seg, ratio);
			case PlayMode.Catmull:
				return CatmullPosition(seg, ratio);
		}
	}
    
	/**
	* Allows switching between a linear movement and a catmull movement for the rail
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	* @param mode The movement mode
	*/
	public Vector3 PositionOnRail(int i, int j, float ratio, PlayMode mode) {
		return LinearPosition(i, j, ratio);
	}
	
	/**
	* Linearly interpolates from one position to another.
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	*/
	public Vector3 LinearPosition(int seg, float ratio) {
		Vector3 p1 = nodes[seg].position;
		Vector3 p2 = nodes[seg + 1].position;
		
		return Vector3.Lerp(p1, p2, ratio);
	}
	
	/**
	* Linearly interpolates from one position to another.
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	*/
	public Vector3 LinearPosition(int i, int j, float ratio) {
		Vector3 p1 = nodes[i].position;
		Vector3 p2 = nodes[j].position;
		
		return Vector3.Lerp(p1, p2, ratio);
	}
	
	/**
	* Smooths out the transition between points 
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	*/
	public Vector3 CatmullPosition(int seg, float ratio) {
		Vector3 p1, p2, p3, p4;
		p1 = p2 = p3 = p4 = Vector3.zero;
		
		if (seg == 0) {
			p1 = nodes[seg].position;
			p2 = p1;
			p3 = nodes[seg + 1].position;
			p4 = nodes[seg + 2].position;
		} else if (seg == nodes.Count - 2) {
			p1 = nodes[seg - 1].position;
			p2 = nodes[seg].position;
			p3 = nodes[seg + 1].position;
			p4 = p3;
		} else {
			p1 = nodes[seg - 1].position;
			p2 = nodes[seg].position;
			p3 = nodes[seg + 1].position;
			p4 = nodes[seg + 2].position;
		}
		
		float t2 = ratio * ratio;
		float t3 = t2 * ratio;
		
		float x =
		0.5f * ((2.0f * p2.x) +
		(-p1.x + p3.x) * ratio +
		(2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) * t2
		+ (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);
		
		float y =
		0.5f * ((2.0f * p2.y) +
		(-p1.y + p3.y) * ratio +
		(2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y) * t2 +
		(-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y) * t3);
		
		float z =
		0.5f * ((2.0f * p2.z) +
		(-p1.z + p3.z) * ratio +
		(2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z) * t2 +
		(-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z) * t3);
		
		return new Vector3(x, y, z);
	}
	
	
	/**
	* Linearly rotates from one quaternion to another.
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	*/
	public Quaternion Orientation(int seg, float ratio) {
		Quaternion q1 = nodes[seg].rotation;
		Quaternion q2 = nodes[seg + 1].rotation;
		
		return Quaternion.Lerp(q1, q2, ratio);
	}
	
	/**
	* Linearly rotates from one quaternion to another.
	*
	* @param seg The line segment we are on
	* @param ratio The ratio is from 0 to 1, 0 being on the beginning of the line segment and 1 being on the end of the line segment
	*/
	public Quaternion Orientation(int i, int j, float ratio) {
		Quaternion q1 = nodes[i].rotation;
		Quaternion q2 = nodes[j].rotation;
		
		return Quaternion.Lerp(q1, q2, ratio);
	}
	
    // Draw a dotted line between the nodes in the editor
	private void OnDrawGizmos() {
		if (nodes.Count > 0) {
#if (UNITY_EDITOR)
            Handles.color = Color.white;
			for (int i = 0; i < nodes.Count - 1; i++) {
				Handles.DrawDottedLine(nodes[i].position, nodes[i+1].position, 3.0f);
			}
			Handles.color = Color.red;
			for (int i = 0; i < nodes.Count; i++) {
				TransitionPoint tp = nodes[i].gameObject.GetComponent<TransitionPoint>();
				if (tp.targetPoint != null) {
					Handles.DrawDottedLine(nodes[i].position, nodes[nodes.IndexOf(tp.targetPoint.transform)].position, 3.0f);
				}
            }
#endif
        }
    }
}