using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour {

    [SerializeField]
    Color healthyColor;
    [SerializeField]
    Color deadColor;

    Light light;
    float health;

    public static HealthIndicator reference;

    void Awake()
    {
        reference = this;
    }

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        float t = health / 100.0f;
        // Debug.Log(t);
        light.color = Color.Lerp(deadColor, healthyColor, (health / 100.0f));
	}

    public void SetHealth(float value)
    {
        health = value;
    }
}
