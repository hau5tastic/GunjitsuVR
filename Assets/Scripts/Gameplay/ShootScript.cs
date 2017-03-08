using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SteamVR_TrackedController))]
public class ShootScript : MonoBehaviour {
    private SteamVR_TrackedController _controller;

    RaycastHit hit;
    
    void Awake()
    {
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
    }

    private void OnEnable()
    {
        _controller = GetComponent<SteamVR_TrackedController>();
        _controller.TriggerClicked += Shoot;
    }

    private void OnDisable()
    {
        _controller.TriggerClicked -= Shoot;
    }


    public void Shoot(object sender, ClickedEventArgs e)
    {

        //// Play SFX
        //audioSource.PlayOneShot(gunshotSFX, 0.3f);

        Debug.Log("Gun/Shoot()");
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            Debug.Log("Gun/Shoot() -  Raycasted against " + hit.transform.tag);
            if (hit.transform.tag == "Note")
            {
                //hit.transform.GetComponent<Note>().destroyed = true;
            }
            //Rigidbody r = hit.collider.GetComponent<Rigidbody>();
            //if (r)
            //    r.GetComponent<EnemyScript>().Kill();
        }



    }

}
