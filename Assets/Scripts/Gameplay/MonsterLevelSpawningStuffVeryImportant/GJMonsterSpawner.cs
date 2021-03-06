﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonsterSpawner : MonoBehaviour {

    void Start() {
        NormalizePosition();

        float d = Vector3.Distance(transform.position, Camera.main.transform.position);
        if ((int)d != GJLevel.instance.spawnRange)
            Debug.LogWarning("SpawnerRange settings do not match play settings, Song might be desynced: set: "
                + GJLevel.instance.spawnRange + " actual: " + d);
    }

    public void Spawn(GameObject monsterPrefab, int noteType, float killTime) {
        GameObject go = Instantiate(monsterPrefab, transform.position - Vector3.up * 1.2f, Quaternion.identity);
        go.GetComponent<GJMonster>().killTime = killTime;
        if ((Note.NOTE_TYPE)noteType == Note.NOTE_TYPE.LEFT)
        {
            GJGuideReticle.lReticleQ.Enqueue(go);
        } else //Assume right :7
        {
            GJGuideReticle.rReticleQ.Enqueue(go);
        }
    }

    void NormalizePosition() {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.position = Camera.main.transform.position + direction.normalized * (GJLevel.instance.spawnRange);
    }
}
