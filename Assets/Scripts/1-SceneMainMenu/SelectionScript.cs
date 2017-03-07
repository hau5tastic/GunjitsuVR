using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour {

    public GameObject spotLightPrefab;
    public string selectionLabel;
    public Color labelColor;
    public Font labelFont;

    Transform centerLight;
    GameObject spotLight;

	void Start () {
        CreateLabel();

        centerLight = GameObject.Find("CenterLight").transform;
        CreateSpotlight();
    }

    void CreateLabel() {
        GameObject label = new GameObject("SelectionLabel");
        Text labelText = label.AddComponent<Text>();

        labelText.text = selectionLabel;
        labelText.color = labelColor;
        labelText.font = labelFont;
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.fontSize = 6;
        labelText.rectTransform.localScale *= 0.1f;
        label.transform.position = transform.position + transform.up * 3f;
        label.transform.SetParent(GameObject.Find("WorldCanvas").transform);
        label.transform.LookAt(Camera.main.transform);
        label.transform.Rotate(0f, 180f, 0f);

        ContentSizeFitter labelFitter = label.AddComponent<ContentSizeFitter>();
        labelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        labelFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    void CreateSpotlight() {
        spotLight = Instantiate(spotLightPrefab, centerLight);
        spotLight.transform.localPosition = Vector3.zero;
        spotLight.transform.LookAt(transform);
    }


	
}
