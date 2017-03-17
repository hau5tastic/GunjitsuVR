using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    float speed = 25.0f;

	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
