using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {

    // public float radius;
    [Range(1f, 360f)]
    public float angle;


    void OnDrawGizmos() {
        for (int i = 0; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            t.localRotation = Quaternion.Euler(0, (i * angle) / (transform.childCount), 0);
            t.position = transform.position - t.transform.forward * GameSettings.spawnRange;
            // Debug.Log("i: "+i+" = " +t.gameObject.name);

        }

        for (int i = 0; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            Debug.DrawLine(transform.position, t.position, Color.red);
        }

    }
}
