using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    SpawnerScript[] spawnPoints;

    public GameObject enemy;
    [SerializeField]
    const float SPAWN_RATE = 0.5f;
    [SerializeField]
    float spawnTime = SPAWN_RATE;

    int numSpawnpoints;
    int currentSpawn = 0;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnerScript>();
        Debug.Log(spawnPoints.Length);
        numSpawnpoints = spawnPoints.Length;
    }

    void Update()
    {
        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0)
        {
            Instantiate(enemy, spawnPoints[currentSpawn].transform.position, Quaternion.identity);
            spawnTime = SPAWN_RATE;
            currentSpawn++;

            if(currentSpawn == numSpawnpoints)
            {
                currentSpawn = 0;
            }
        }
    }
}
