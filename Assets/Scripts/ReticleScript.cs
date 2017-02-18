using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReticleScript : MonoBehaviour {

    public Sprite LHandSprite;
    public Sprite RHandSprite;

    const int INNER = 0;
    const int OUTER = 1;
    const int HAND = 2;

    [SerializeField]
    GameObject targetObject;

    public float startReticleSize = 50f;
    public float endReticleSize = 10f;
    public float criticalValue = 0.7f;

    [Range(0.0f, 1.0f)]
    public float completionValue;

    [Header("Range")]
    public float arrivalRange; // If target is outside of arrivalRange, it is hidden and has the startReticleSize, 
                               // if the target's position is == camera's position, it has endReticleSize

    Image[] reticles;
    new RectTransform transform;

    public AudioClip lockOnSFX;


    void Awake()
    {
        transform = GetComponent<RectTransform>();
        reticles = GetComponentsInChildren<Image>();   
    }

    void Start()
    {
        transform.LookAt(Camera.main.transform);
        reticles[INNER].rectTransform.sizeDelta = new Vector2(startReticleSize * .5f, startReticleSize * .5f);
        reticles[OUTER].rectTransform.sizeDelta = new Vector2(startReticleSize, startReticleSize);
    }

    void Update()
    {
        if (targetObject == null) Destroy(gameObject);
        // if (transform.parent == null) Destroy(gameObject);
        if (targetObject.GetComponent<Renderer>().enabled == false) Destroy(gameObject);
        

        // Set the reticle position in front of the target by 20% of the objects overall thickness
        Vector3 direction = Vector3.Normalize(Camera.main.transform.position - targetObject.transform.position);
        Vector3 newPosition = targetObject.transform.position + direction * (targetObject.GetComponent<Renderer>().bounds.size.z * 1.2f);
        transform.position = newPosition;

        /// Update the completion Value of the reticle (0-1)
        // Grab the distance between the camera and the target
        float dist = Vector3.Distance(Camera.main.transform.position, targetObject.transform.position);
        if (dist <= arrivalRange)
        {
            targetObject.GetComponent<Renderer>().material.color = Color.white;
            completionValue = 1f - (dist / arrivalRange);

            /// Events that can happen at certain completion points
            // If the target is almost at the end point...
            if (completionValue >= criticalValue)
            {
                targetObject.GetComponent<Renderer>().material.color = new Color(.5f,0f, 0f,1);  // Change its color to red
                // Destroy(gameObject);
            } else
            {
                reticles[INNER].rectTransform.Rotate(Vector3.forward, -0.7f); // While its not at 90% keep rotating the inner reticle.
            }

        }
        reticles[OUTER].rectTransform.Rotate(Vector3.forward, 0.7f);

        // Lerp the rotation values based on completion value
        reticles[INNER].rectTransform.sizeDelta = Vector2.Lerp(new Vector2(startReticleSize * .5f, startReticleSize * .5f), new Vector2(endReticleSize * .5f, endReticleSize * .5f), completionValue);
        reticles[OUTER].rectTransform.sizeDelta = Vector2.Lerp(new Vector2(startReticleSize, startReticleSize), new Vector2(endReticleSize, endReticleSize), completionValue);
        transform.LookAt(Camera.main.transform);

        reticles[HAND].transform.LookAt(Camera.main.transform);

        // Lerp the color values based on completion value
        for (int i = 0; i < 2; ++i)
        {
            reticles[i].color = Color.Lerp(new Color(0,0,0,0), Color.red, completionValue);
        }
        reticles[HAND].color = Color.Lerp(new Color(1, 1, 1, 0.5f), Color.white, completionValue);
    }

    public void SetTarget(GameObject _targetObject)
    {
        targetObject = _targetObject;
    }
}