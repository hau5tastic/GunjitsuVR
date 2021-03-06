﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJSpawnerSpacer : MonoBehaviour {

    public float rotationSpeed;
    public float spreadSpeed;
    public float elevationSpeed;
    public float angle;

    public void SetRange(float spawnRange) {
        for (int i = 0; i < transform.childCount; ++i) {
            Transform t = transform.GetChild(i);
            t.localRotation = Quaternion.Euler(0, (i * angle) / (transform.childCount), 0);
            t.position = transform.position + (t.transform.forward * spawnRange);
            Debug.DrawLine(transform.position, t.position, Color.red);
        }
    }

    void FixedUpdate() {
        Rotate();
        Spread();
        Elevate();
        SetRange(GJLevel.instance.spawnRange);
    }

    // Prototype Controls .. Faking 360
    public void Rotate() {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void Spread() {
        angle += spreadSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle, 70, 90);
    }

    public void Elevate() {
        Vector3 delta = Vector3.up * elevationSpeed * Time.deltaTime;
        transform.position += delta;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 3, 10), transform.position.z);
    }
    
}
