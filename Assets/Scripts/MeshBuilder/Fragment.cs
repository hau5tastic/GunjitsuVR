using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour {

    public GameObject builtObject;
    Vector3 startPosition;
    Quaternion startRotation;

    public float maxDuration;
    public Vector3 endPosition;
    public Quaternion endRotation;

    float time = 0;
    float duration;
    float completion;

    void Start() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        duration = Random.Range(1.0f, maxDuration);
        // GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;


    }

    void Update() {

        time += Time.deltaTime;

        completion = time / duration;
        transform.position = Vector3.Slerp(startPosition, endPosition, completion);
        transform.rotation = Quaternion.Slerp(startRotation, endRotation, completion);

        if (time >= maxDuration) {
            if (!builtObject.activeSelf) {
                // Instantiate(pulseLightPrefab, builtObject.transform.position, Quaternion.identity);
                builtObject.SetActive(true);
            }

            Destroy(gameObject);
        }
    }
}
