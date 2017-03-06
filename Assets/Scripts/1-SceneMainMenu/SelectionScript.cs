using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour {

    public string labelText;
    public float rotationSpeed;
    GameObject selectionSpotlight;
    GameObject UILabel;

	void Start () {
        selectionSpotlight = new GameObject("Selection Spotlight");
        Light lightComp = selectionSpotlight.AddComponent<Light>();
        lightComp.color = Color.white;
        lightComp.type = LightType.Spot;
        lightComp.spotAngle = 30;
        lightComp.range = 90;
        lightComp.shadows = LightShadows.Hard;
        lightComp.intensity = 2.0f;

        selectionSpotlight.transform.position = new Vector3(0, 10, 0);
        selectionSpotlight.transform.LookAt(transform);

        UILabel = new GameObject("Selection Label");
        Text textComp = UILabel.AddComponent<Text>();

        UILabel.GetComponent<RectTransform>().SetParent(GameObject.Find("WorldCanvas").transform);
        UILabel.GetComponent<RectTransform>().position = new Vector3(transform.position.x, 5, transform.position.y);
        textComp.text = labelText;
        textComp.font = (Font)Resources.Load<Font>("Fonts/Gunplay rd.ttf");
    }
	
	void Update () {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        UILabel.GetComponent<RectTransform>().LookAt(Camera.main.transform);

	}
}
