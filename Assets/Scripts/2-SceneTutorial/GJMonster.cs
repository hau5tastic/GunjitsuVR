using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonster : MonoBehaviour {

    public GameObject particlePrefab;
    public GameObject indicatorPrefab;
    public bool paused = false;

	void Start () {

        GetComponent<Rigidbody>().freezeRotation = true;
        GameObject go = Instantiate(indicatorPrefab, GameObject.Find("WorldCanvas").transform, true);
        go.transform.position = transform.position;
        go.GetComponent<GJShootIndicator>().target = gameObject;
        transform.LookAt(Camera.main.transform);

    }
	
	void Update () {
        if (paused) return;
        
        transform.position += transform.forward * GameSettings.playSpeed * Time.deltaTime;

        if (IsWithinKillRange()) Kill();
	}

    bool IsWithinKillRange() {
        return (Vector3.Distance(Camera.main.transform.position, transform.position) <= GameSettings.killRange/4.0f);
    }

    public void Kill()
    {
        Destroy(Instantiate(particlePrefab, transform.position, Quaternion.identity), .5f);
        Destroy(gameObject);
    }
}
