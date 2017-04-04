using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour {

    [SerializeField]
    Color healthyColor;
    [SerializeField]
    Color deadColor;

    Material terrain;
    float health;

    public static HealthIndicator reference;

    void Awake()
    {
        reference = this;
    }

	// Use this for initialization
	void Start () {
        terrain = GetComponent<Terrain>().materialTemplate;
        terrain.SetColor("_EmissionColor", healthyColor);
	}
	
	// Update is called once per frame
	void Update () {
        float t = health / 100.0f;
        // Debug.Log(t);
        terrain.SetColor("_EmissionColor", Color.Lerp(deadColor, healthyColor, (health / 100.0f)));
	}

    public void SetHealth(float value)
    {
        health = value;
    }
}
