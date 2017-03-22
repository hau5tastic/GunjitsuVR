﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonsterSpawner : MonoBehaviour {


    void Start() {
        NormalizePosition();

        int d = (int)Vector3.Distance(transform.position, Camera.main.transform.position);
        if (d != GameSettings.spawnRange)
            Debug.LogWarning("SpawnerRange settings do not match play settings, Song might be desynced: set: "
                + GameSettings.spawnRange + " actual: " + d);
    }

    public void Spawn(GameObject monsterPrefab, int noteType) {
        if ((Note.NOTE_TYPE)noteType == Note.NOTE_TYPE.LEFT)
        {
            GJGuideReticle.lReticleQ.Enqueue(Instantiate(monsterPrefab, transform.position, Quaternion.identity));
        } else //Assume right :7
        {
            GJGuideReticle.rReticleQ.Enqueue(Instantiate(monsterPrefab, transform.position, Quaternion.identity));
        }
    }

    void NormalizePosition() {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.position = Camera.main.transform.position + direction.normalized * (GameSettings.spawnRange);
    }
}
