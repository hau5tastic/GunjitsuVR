using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootScript : MonoBehaviour {
    [SerializeField]
    SteamVR_TrackedController _controller;

    RaycastHit hit;

    AudioSource audioSource;


    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position + new  Vector3(0,0.085f,0), transform.forward * 100f, Color.red);
    }

    private void OnEnable()
    {
        _controller.TriggerClicked += Shoot;
    }

    private void OnDisable()
    {
        _controller.TriggerClicked -= Shoot;
    }


    public void Shoot(object sender, ClickedEventArgs e)
    {

        //// Play SFX
        audioSource.PlayOneShot(audioSource.clip, 0.3f);

        Debug.Log("Gun/Shoot() - Raycast attempt");
        if (Physics.Raycast(transform.position + new Vector3(0, 0.085f, 0), transform.forward, out hit, 100f))
        {
            Debug.Log("Gun/Shoot() -  Raycasted against " + hit.transform.tag);
            if (hit.transform.tag == "GJMonster")
            {
                hit.collider.GetComponent<GJMonster>().Kill();
                //hit.transform.GetComponent<Note>().destroyed = true;
            }
            //Rigidbody r = hit.collider.GetComponent<Rigidbody>();
            //if (r)
            //    r.GetComponent<EnemyScript>().Kill();
        }



    }

}
