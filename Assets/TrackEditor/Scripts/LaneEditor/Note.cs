// ------------------------------------------------------------
// Note - A single note within a Lane. 
// ------------------------------------------------------------
using UnityEngine;

public class Note : MonoBehaviour
{
    // ------------------------------------------------------------
    MeshRenderer meshRenderer;

    public float initialPosition { get; set; }

    public bool destroyed { get; set; }

    // ------------------------------------------------------------
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        destroyed = false;
    }
    // ------------------------------------------------------------
    void Update()
    {
        //TODO editor only
        HandleNoteDeletion();
    }
    // ------------------------------------------------------------
    // Called by the managing Lane!
    public void UpdatePosition(Vector3 newPosition, bool visible)
    {
        //TODO!! garbage code incoming
            //please move this code elsewhere
        if (destroyed) {
            GetComponent<Collider>().enabled = false;
            meshRenderer.enabled = false;
            return;
        }

        meshRenderer.enabled = visible;
        if (visible) {
            this.transform.position = newPosition;
        }
    }
    // ------------------------------------------------------------
    void HandleNoteDeletion()
    {
        //TODO Editor FindObjectOfType once instead of per frame;
        //TODO reduce redundantly
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = FindObjectOfType<Camera>();

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject != this.gameObject) return;
                this.GetComponentInParent<Lane>().removeNote(this);
            }
        }
    }
    // ------------------------------------------------------------
}
