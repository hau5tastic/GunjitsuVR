using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GJScore;

public class GJMonster : MonoBehaviour {

    public GameObject particlePrefab;
    public GameObject indicatorPrefab;
    public GameObject scorePrefab;
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

        if (IsWithinKillRange()) Kill(false);
	}

    bool IsWithinKillRange() {
        return (Vector3.Distance(Camera.main.transform.position, transform.position) <= GameSettings.killRange - 0.5f);
    }

    public void Kill(bool killedByShot)
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Destroy(particle, .5f);

        if (killedByShot)
        {
            CreateScorePopup();
            GJLevel.hitCount++;
        } else {
            particle.GetComponent<AudioSource>().Play();
            GJLevel.synchronization-=5;
        }

        Destroy(gameObject);
    }

    void CreateScorePopup()
    {
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        GameObject go = Instantiate(scorePrefab, GameObject.Find("ScoreCanvas").transform, true);
        go.transform.position = transform.position + (transform.up * 1.2f);

        if (distance <= GameSettings.perfect)
        {
            ScoreText.reference.AddScore(GameSettings.perfectScore);
            go.GetComponent<GJScorePopup>().Init(GJAccuracy.PERFECT);
        }
        else if(distance <= GameSettings.great)
        {
            ScoreText.reference.AddScore(GameSettings.greatScore);
            go.GetComponent<GJScorePopup>().Init(GJAccuracy.GREAT);
        }
        else if(distance <= GameSettings.good)
        {
            ScoreText.reference.AddScore(GameSettings.goodScore);
            go.GetComponent<GJScorePopup>().Init(GJAccuracy.GOOD);
        }
        else
        {
            ScoreText.reference.AddScore(GameSettings.okScore);
            go.GetComponent<GJScorePopup>().Init(GJAccuracy.OK);
        }

        Debug.Log(distance);

        go.transform.LookAt(Camera.main.transform);
        go.transform.Rotate(0, 180, 0);
    }
}
