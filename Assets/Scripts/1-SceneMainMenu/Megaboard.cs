using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaboard : MonoBehaviour {

    public float delay;
    public float speed;
    public Vector3 endPos;


	void Update () {
        delay -= Time.deltaTime;
        if (delay <= 0) {
            transform.position = Vector3.Lerp(transform.position, endPos, speed * Time.deltaTime);
        }
	}
}
