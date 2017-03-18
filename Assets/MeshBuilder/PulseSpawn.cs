using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PulseSpawn : MonoBehaviour {

    public float pulseSpeed;
    public float pulseDistance = 0f;

    public List<GameObject> gameObjects;
    Queue<GameObject> pulseQueue;

    void Awake() {
        gameObjects = new List<GameObject>();
        gameObjects.AddRange(FindObjectsOfType<GameObject>());
        reinitialize();
    }

    void reinitialize() {
        // Sort gameObjects based on distance to player -- Using Linq (more efficient I think..)
        gameObjects = gameObjects.OrderBy(x =>
            Vector3.Distance(this.transform.position, x.transform.position)).ToList();

        pulseQueue = new Queue<GameObject>();
        foreach (GameObject go in gameObjects) {
            if (LayerMask.LayerToName(go.layer) == "MeshBuild") 
            {
                pulseQueue.Enqueue(go);
                go.SetActive(false);
            }
            //if (go.GetComponent<MeshFilter>() == null) continue;
            //if (go == this.gameObject) continue;
            //if (go.name == "[CameraRig]") continue;
        }

    }

    GameObject currentObject;
    void Update() {

        if (pulseQueue.Count > 0) {
            pulseDistance += pulseSpeed * Time.deltaTime;
        }
        else {
            return;
        }

        currentObject = pulseQueue.Peek();
        float d = Vector3.Distance(transform.position, currentObject.transform.position);
        if (d <= pulseDistance) {
            if (currentObject.layer == 31) return;
            currentObject.SetActive(true);
            BreakMesh bm = currentObject.AddComponent<BreakMesh>();
            bm.minDuration = 1.0f;
            bm.maxDuration = 5.0f;
            // Instantiate(pulseLightPrefab, currentObject.transform, false);
            pulseQueue.Dequeue();
        }


    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pulseDistance);
    }
}



/*
// Sort gameObjects based on distance to player
gameObjects.Sort(delegate(GameObject a, GameObject b) {
    return Vector2.Distance(transform.position, a.transform.position).CompareTo(
 Vector2.Distance(transform.position, b.transform.position));
});
*/
