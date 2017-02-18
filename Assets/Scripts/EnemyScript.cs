using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    GameObject g;

    public GameObject reticlePrefab;

	void Start () {
        g = GameObject.FindGameObjectWithTag("Player");
        GameObject reticle = Instantiate(reticlePrefab) as GameObject;
        reticle.transform.SetParent(GameObject.Find("WorldCanvas").transform);

        reticle.GetComponent<ReticleScript>().SetTarget(gameObject);
	}
	
	void Update () {
        transform.LookAt(g.transform);
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}
