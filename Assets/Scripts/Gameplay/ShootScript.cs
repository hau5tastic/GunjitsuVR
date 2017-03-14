using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        TestRaycast();
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
            }
        }
    }

    public void TestRaycast()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            //WorldUI is my layer name
            if (results[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                string dbg = "Root Element: {0} \n GrandChild Element: {1}";
                Debug.Log(string.Format(dbg, results[results.Count - 1].gameObject.name, results[0].gameObject.name));
                //Debug.Log("Root Element: "+results[results.Count-1].gameObject.name);
                //Debug.Log("GrandChild Element: "+results[0].gameObject.name);
                results.Clear();
            }
        }
    }
}
