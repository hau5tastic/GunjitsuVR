using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJSpacerController : MonoBehaviour {

    public float interval;
    GJSpawnerSpacer spacer;
    void Awake() {
        spacer = GameObject.FindObjectOfType<GJSpawnerSpacer>();
    }

    void Start() {
        StartCoroutine(Perform());
    }

    IEnumerator Perform() {
        while (true) {
            int random = Random.Range(0, 6);
            switch (random) {
                case 0:
                    spacer.rotationSpeed = 5;
                    break;
                case 1:
                    spacer.rotationSpeed = -5;
                    break;
                case 2:
                    spacer.spreadSpeed = 5;
                    break;
                case 3:
                    spacer.spreadSpeed = -5;
                    break;
                case 4:
                    spacer.elevationSpeed = 5;
                    break;
                case 5:
                    spacer.elevationSpeed = -5;
                    break;
                default:
                    break;
            }

            Debug.Log("Performing...");
            yield return new WaitForSeconds(interval);
        }
    }
}
