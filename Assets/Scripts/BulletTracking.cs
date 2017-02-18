using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracking : MonoBehaviour {

    // Creates a line renderer that follows a Sin() function
    // and animates it.

    LineRenderer lineRenderer;
    public Color color = Color.yellow;
    public int lengthOfLineRenderer = 3;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material.color = color;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.numPositions = lengthOfLineRenderer;
    }

    void Update()
    {
        //for (int i = 0; i < lengthOfLineRenderer; i++)
        //{
        //    lineRenderer.SetPosition(i, new Vector3(i, i, 0.0f));
        //}

        //lineRenderer.SetPosition(0, new Vector3(0, 0, 1.0f));
        //lineRenderer.SetPosition(1, new Vector3(0, 0, 100.0f));
    }
}
