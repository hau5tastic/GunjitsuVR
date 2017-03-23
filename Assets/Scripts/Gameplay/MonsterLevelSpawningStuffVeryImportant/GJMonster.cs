using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonster : MonoBehaviour {

    public GameObject killedParticlePrefab;
    public GameObject killParticlePrefab;
    public GameObject indicatorPrefab;
    public GameObject scorePrefab;
    public bool paused = false;

    void Awake()
    {
        if(GJLevel.instance.autoPlaySong)
        {

            StartCoroutine(AutoKill());
        }
        else
        {
            //GetComponent<AudioSource>().clip.LoadAudioData();
            GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + TimeToDeath(true));
        }
    }

    void Start () {
        // GetComponent<Rigidbody>().freezeRotation = true;
        // indicators
        //GameObject go = Instantiate(indicatorPrefab, GameObject.Find("WorldCanvas").transform, true);
        //go.transform.position = transform.position;
        //go.GetComponent<GJShootIndicator>().target = gameObject;
        transform.LookAt(GJLevel.instance.center);
    }
	
	void Update () {
        if (paused) return;
        
        transform.position += transform.forward * GJLevel.instance.overridePlaySpeed * Time.deltaTime;

        if (IsWithinKillRange()) Kill(false);
	}

    bool IsWithinKillRange() {
        return (Vector3.Distance(GJLevel.instance.center.position, transform.position) <= GJLevel.instance.killRange - 1.5f);
    }

    public void Kill(bool killedByShot)
    {
        if (killedByShot)
        {
            GetComponent<AudioSource>().Stop(); // stop scheduled death sound
            CreateScorePopup();
            GJLevel.instance.hitCount++;
            GameObject particle = Instantiate(killedParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, .5f);
        } else {
            GJLevel.instance.synchronization -= 5;
            GameObject particle = Instantiate(killParticlePrefab, transform.position, Quaternion.identity);
            //particle.GetComponent<AudioSource>().Play();
            Destroy(particle, .5f);
        }

        SplitMeshIntoTriangles splitter = GetComponentInChildren<SplitMeshIntoTriangles>();
        if (splitter)
        {
            splitter.SplitMesh();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AutoKill()
    {
        //Debug.Log("Time to death: " + TimeToDeath(false));
        yield return new WaitForSeconds(TimeToDeath(false));
        Kill(true);
        StopAllCoroutines();
    }

    void CreateScorePopup()
    {
        float distance = Mathf.Abs(Vector3.Distance(Camera.main.transform.position, transform.position) - GJLevel.instance.killRange);

        GameObject go = Instantiate(scorePrefab, GameObject.Find("ScoreCanvas").transform, true);
        go.transform.position = transform.position + (transform.up * 1.2f);

        if (distance <= GJLevel.instance.perfect)
        {
            ScoreText.reference.AddScore(GJLevel.instance.perfectScore);
            go.GetComponent<GJScorePopup>().Init(GJLevel.GJAccuracy.PERFECT);
        }
        else if(distance <= GJLevel.instance.great)
        {
            ScoreText.reference.AddScore(GJLevel.instance.greatScore);
            go.GetComponent<GJScorePopup>().Init(GJLevel.GJAccuracy.GREAT);
        }
        else if(distance <= GJLevel.instance.good)
        {
            ScoreText.reference.AddScore(GJLevel.instance.goodScore);
            go.GetComponent<GJScorePopup>().Init(GJLevel.GJAccuracy.GOOD);
        }
        else
        {
            ScoreText.reference.AddScore(GJLevel.instance.okScore);
            go.GetComponent<GJScorePopup>().Init(GJLevel.GJAccuracy.OK);
        }

        //Debug.Log("DEATH DISTANCE: " + distance);

        go.transform.LookAt(Camera.main.transform);
        go.transform.Rotate(0, 180, 0);
    }

    float TimeToDeath(bool useOffset)
    {
        float distance;

        distance = Vector3.Distance(GJLevel.instance.center.position, transform.position) - (GJLevel.instance.killRange);
        
        return (distance / GJLevel.instance.overridePlaySpeed);
    }
}
