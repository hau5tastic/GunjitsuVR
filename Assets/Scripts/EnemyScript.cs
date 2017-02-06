using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    GameObject g;
    Rigidbody r;

	void Start () {
        g = GameObject.FindGameObjectWithTag("Player");
        r = GetComponent<Rigidbody>();

        Destroy(gameObject, 2.5f);

	}
	
	void Update () {
        transform.LookAt(g.transform);
        r.AddForce(transform.forward * 0.3f, ForceMode.VelocityChange);
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}
