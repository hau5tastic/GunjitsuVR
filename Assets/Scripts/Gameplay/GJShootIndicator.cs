using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GJShootIndicator : MonoBehaviour {

    public GameObject target;

    public float heightOffset;
    public float lockTime;

    float currentTime;
    float startDistance;

    Image[] indicators;


	void Start () {
        indicators = GetComponentsInChildren<Image>();
        startDistance = GameSettings.killRange + (GameSettings.playSpeed * lockTime);
        indicators[0].color = new Color(1, 1, 1, 0);
        indicators[1].color = new Color(1, 1, 1, 0);
        // transform.Rotate(90, 0, 0); // For Underneath Indication

    }
	
	void Update () {
        if (!target) {
            Destroy(gameObject);
            return;
        }


        transform.LookAt(Camera.main.transform); // Disable this if underneath indication
        transform.position = target.transform.position + (target.transform.up * heightOffset);

        if (Vector3.Distance(target.transform.position, Camera.main.transform.position) <= startDistance) {
            indicators[0].color = new Color(1, 1, 1, 1);
            indicators[1].color = new Color(1, 1, 1, 1);
            currentTime += Time.deltaTime;
            float completion = currentTime / lockTime;

            if (completion >= 1f)
            {
                Destroy(gameObject);
            }
            indicators[0].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1) * completion;
        }



	}
}
