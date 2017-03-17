﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ShootScript : MonoBehaviour {
    [SerializeField]
    SteamVR_TrackedController _controller;

    public GameObject bullet;

    RaycastHit hit;

    AudioSource audioSource;

    private Camera UICamera;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //// Create a new camera that will be used for raycasts
        //UICamera = new GameObject("UI Camera").AddComponent<Camera>();
        //UICamera.clearFlags = CameraClearFlags.Nothing;
        //UICamera.cullingMask = 0;
        //UICamera.fieldOfView = 5;
        //UICamera.nearClipPlane = 0.01f;

        //// Find canvases in the scene and assign our custom UICamera to them
        //Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        //foreach (Canvas canvas in canvases)
        //{
        //    canvas.worldCamera = UICamera;
        //}
    }

    void Update()
    {
        //Debug.DrawRay(transform.position + new Vector3(0,0.085f,0), transform.forward * 100f, Color.red);

        //TestRaycast();
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

        Destroy(Instantiate(bullet, transform.position + transform.forward * 0.2f + transform.up * 0.05f, transform.rotation), 2.0f);

        Debug.Log("ShootScript/Shoot() - Raycast attempt");
        if (Physics.Raycast(transform.position + new Vector3(0, 0.085f, 0), transform.forward, out hit, 100f))
        {


            Debug.Log("ShootScript/Shoot() -  Raycasted against " + hit.transform.tag);
            if (hit.transform.tag == "GJMonster")
            {
                hit.collider.GetComponent<GJMonster>().Kill(true);
            }
        }
    }

    public void TestRaycast()
    {
        Vector3 rayDirection = transform.rotation * Vector3.forward;
        Debug.DrawRay(transform.position, rayDirection, Color.red);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = rayDirection;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            //Debug.Log("ShootScript/TestRaycast() - Raycast Results Found " + results.Count);
            //WorldUI is my layer name;
            if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                results.Clear();
            }
        } else
        {
            //Debug.Log("ShootScript/TestRaycast() - No Targets Found.");
        }
    }
}
