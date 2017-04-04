using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroParticle : MonoBehaviour {

    float speed;
    float turnInterval;
    Vector3 mainDirection;

    Vector3 currentDirection;

	void Start () {
        speed = 20f;
        turnInterval = Random.Range(0.5f, 1.0f);
        mainDirection = Vector3.forward;
        currentDirection = mainDirection;
        StartCoroutine(Perform());

        int r = Random.Range(0, 4);
        switch (r) {
            case 0:
                mainDirection = Vector3.forward;
                break;
            case 1:
                mainDirection = -Vector3.forward;
                break;
            case 2:
                mainDirection = Vector3.right;
                break;
            case 3:
                mainDirection = -Vector3.right;
                break;
            default:
                break;
        }
	}

    void FixedUpdate() {
        transform.position += currentDirection * speed * Time.deltaTime;
    }

    IEnumerator Perform() {
        while (true) {
            if (currentDirection == mainDirection) {
                Turn();
            } else {
                currentDirection = mainDirection;
            }
            yield return new WaitForSeconds(turnInterval);
        }
    }

    void Turn() {
        int r = Random.Range(0, 2);
        if (mainDirection == Vector3.forward || mainDirection == -Vector3.forward) {
            currentDirection = (r == 0) ? Vector3.right : -Vector3.right;
        }
        else {
            currentDirection = (r == 0) ? Vector3.forward : -Vector3.forward;
        }
    }

}
