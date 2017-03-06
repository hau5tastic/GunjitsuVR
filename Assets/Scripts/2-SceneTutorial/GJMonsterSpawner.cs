using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonsterSpawner : MonoBehaviour {


    void Start() {
        NormalizePosition();
    }

    public void Spawn(GameObject monsterPrefab) {
        GJGuideReticle.rReticleQ.Enqueue(Instantiate(monsterPrefab, transform.position, Quaternion.identity));
    }

    void NormalizePosition() {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.position = Camera.main.transform.position + direction.normalized * GameSettings.spawnRange;
    }
}
