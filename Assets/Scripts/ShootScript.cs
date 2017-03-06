using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ShootScript : MonoBehaviour {

    AudioSource audioSource;
    Animation recoilAnim;

    [SerializeField]
    AudioClip gunshotSFX;

    RaycastHit hit;
    
    void Awake()
    {
        recoilAnim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
    }

    public void Shoot()
    {

        // Play SFX
        audioSource.PlayOneShot(gunshotSFX, 0.3f);

        // Play Anim
        if (recoilAnim.IsPlaying("Recoil")) recoilAnim.Stop();
        recoilAnim.Play();


        if (Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
            Debug.Log(hit.transform.tag);
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
