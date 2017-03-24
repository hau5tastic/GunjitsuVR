using System.Collections;
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
        transform.position = Camera.main.transform.position + direction.normalized * (GJLevel.instance.spawnRange);
    }
}
