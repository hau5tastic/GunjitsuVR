using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJScore;

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
        return (Vector3.Distance(Camera.main.transform.position, transform.position) <= GameSettings.killRange);
    }

    public void Kill()
    {
        Destroy(Instantiate(particlePrefab, transform.position, Quaternion.identity), .5f);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        /*ScoreSettings settings = GetComponent<GJScorePopup>().settings;
        if(distance <=  settings.perfect)
        {
            ScoreText.reference.AddScore(settings.perfectScore);
        }
        else if(distance <= settings.great)
        {
            ScoreText.reference.AddScore(settings.greatScore);
        }
        else if(distance <= settings.good)
        {
            ScoreText.reference.AddScore(settings.goodScore);
        }
        else
        {
            ScoreText.reference.AddScore(settings.okScore);
        }*/ 
    }
}
