using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GJRotatingReticle : MonoBehaviour {

    [Header("Reticle Settings")]
    Image[] imageComponents;
    public float size;
    public float sizeOffset;
    public float angleOffset;
    public float angularSpeed;

    void Awake() {
        imageComponents = GetComponentsInChildren<Image>();
        for (int i = 0; i < imageComponents.Length; ++i) {
            float sizeReduction = 1f - (i * sizeOffset);
            imageComponents[i].rectTransform.sizeDelta = 
                new Vector2(size * sizeReduction, size * sizeReduction);

            imageComponents[i].rectTransform.Rotate(new Vector3(0, 0, angleOffset * i));
        }
        
    }

    void Update() {
        GetComponent<RectTransform>().LookAt(Camera.main.transform);
        RotateComponents();
    }

    void RotateComponents() {
        for (int i = 0; i < imageComponents.Length; ++i) {
            float dir = i % 2 == 0 ? 1 : -1;
            imageComponents[i].rectTransform.Rotate(Vector3.forward, angularSpeed * dir * Time.deltaTime);
        }
    }
}
