using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour {

    [SerializeField]
    GJGuideReticle reticle;

    MeshRenderer[] mrends;

    Quaternion newRotation;
    GameObject target;

    float ezDisappearanceNumber;

    void Awake()
    {
        mrends = GetComponentsInChildren<MeshRenderer>();
        SetVisible(false);
    }

    void FixedUpdate () {
        target = reticle.target;

        if (target && !reticle.isOnScreen) //Show this shit
        {
            SetVisible(true);
            transform.LookAt(reticle.transform);
            ezDisappearanceNumber += Time.deltaTime / 2;
            if (ezDisappearanceNumber >= 0.25) //hardcoded bullshit
            {ezDisappearanceNumber = 0.25f;}

        }
        else //Dont show this shit
        {
            SetVisible(false);
            ezDisappearanceNumber -= Time.deltaTime /2;
            if (ezDisappearanceNumber <= 0)
            { ezDisappearanceNumber = 0;}
        }
    }

    void SetVisible(bool value)
    {
        if (ezDisappearanceNumber > 0)
        {
            value = true; //lol
        }

        transform.localScale = Vector3.one * ezDisappearanceNumber;
        foreach (MeshRenderer mr in mrends) {
            mr.enabled = value;
        }

        
    }
}
