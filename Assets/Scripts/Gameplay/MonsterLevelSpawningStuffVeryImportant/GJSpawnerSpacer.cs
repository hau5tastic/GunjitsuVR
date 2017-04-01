using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJSpawnerSpacer : MonoBehaviour {

    public float angle;

    public void Set(float spawnRange) {
        for (int i = 0; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            t.localRotation = Quaternion.Euler(0, (i * angle) / (transform.childCount), 0);
            t.position = transform.position + (t.transform.forward * spawnRange);
            Debug.DrawLine(transform.position, t.position, Color.red);
        }
    }
    
}
