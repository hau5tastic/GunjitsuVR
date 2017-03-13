using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonsterSpawner : MonoBehaviour {


    void Start() {
        NormalizePosition();
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
        transform.position = Camera.main.transform.position + direction.normalized * GameSettings.spawnRange;
    }
}
