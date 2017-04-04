using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJMonster : MonoBehaviour {

    public GameObject killedParticlePrefab;
    public GameObject killParticlePrefab;
    public GameObject indicatorPrefab;

    public float killTime;

    void Awake()
    {
        if(GJLevel.instance.autoPlaySong)
        {

            StartCoroutine(AutoKill());
        }
        else
        {
            //GetComponent<AudioSource>().clip.LoadAudioData();
            GetComponent<AudioSource>().PlayScheduled(AudioSettings.dspTime + TimeToDeath());
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
	
	void FixedUpdate () {
        transform.position += transform.forward * GJLevel.instance.overridePlaySpeed * Time.deltaTime;

        if (IsWithinKillRange()) Kill(false, Vector3.zero);
	}

    bool IsWithinKillRange() {
        return (Vector3.Distance(GJLevel.instance.center.position, transform.position) <= GJLevel.instance.killRange - 1.5f);
    }

    public void Kill(bool killedByShot, Vector3 hitpoint)
    {
        if (killedByShot)
        {
            GetComponent<AudioSource>().Stop(); // stop scheduled death sound
            CreateScorePopup();
            GJLevel.instance.hitCount++;
            GameObject particle = Instantiate(killedParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, .5f);
        } else {
            GJLevel.instance.synchronization -= 3;
            // GameObject particle = Instantiate(killParticlePrefab, transform.position, Quaternion.identity);
            // particle.GetComponent<AudioSource>().Play();
            // Destroy(particle, .5f);
        }

        SplitMeshIntoTriangles splitter = GetComponentInChildren<SplitMeshIntoTriangles>();
        CollapseMesh splitter2 = GetComponentInChildren<CollapseMesh>();
        if (splitter)
        {
            splitter.SplitMesh();
        } else if (splitter2) {
            splitter2.SplitMesh(hitpoint);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator AutoKill()
    {
        //Debug.Log("Time to death: " + TimeToDeath(false));
        yield return new WaitForSeconds(TimeToDeath());
        Kill(true, transform.position);
        StopAllCoroutines();
    }

    void CreateScorePopup()
    {
        
        float songTime = GJLevel.instance.SongTime() - (GJLevel.instance.currentTrack.startOffset / 1000f);
        float deviation = Mathf.Abs(killTime - songTime);

        Vector3 popupPosition = transform.position + (transform.up * 1.2f);
        if (deviation <= GJLevel.instance.perfect)
            GJUIManager.instance.CreatePopupAt(popupPosition, GJLevel.GJAccuracy.PERFECT);
        else if(deviation <= GJLevel.instance.great)
            GJUIManager.instance.CreatePopupAt(popupPosition, GJLevel.GJAccuracy.GREAT);
        else if(deviation <= GJLevel.instance.good)
            GJUIManager.instance.CreatePopupAt(popupPosition, GJLevel.GJAccuracy.GOOD);
        else
            GJUIManager.instance.CreatePopupAt(popupPosition, GJLevel.GJAccuracy.OK);

        Debug.Log("DEATH deviation: " + deviation);


    }

    float TimeToDeath()
    {
        float distance;

        distance = Vector3.Distance(GJLevel.instance.center.position, transform.position) - (GJLevel.instance.killRange);
        
        return (distance / GJLevel.instance.overridePlaySpeed);
    }
}
