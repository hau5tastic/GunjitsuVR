using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMenuScript : MonoBehaviour {

    public float radius;
    [Range(1f, 360f)]
    public float angle;

    void OnDrawGizmos() {
        for (int i = 1; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            t.localRotation = Quaternion.Euler(0, ((i-1) * angle) / (transform.childCount-1), 0);
            t.position = transform.position -t.transform.forward * radius;
            // Debug.Log("i: "+i+" = " +t.gameObject.name);

        }

        for (int i = 1; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            Debug.DrawLine(transform.position, t.position, Color.red);
        }

        Gizmos.DrawCube(transform.GetChild(0).position, Vector3.one);

    }
	
}
