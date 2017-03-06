// ------------------------------------------------------------
// Note - A single note within a Lane. 
// ------------------------------------------------------------
using UnityEngine;
// ------------------------------------------------------------
[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour
{
    // ------------------------------------------------------------
    public float initialPosition { get; set; }
    SpriteRenderer sprite;
    // ------------------------------------------------------------
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    // ------------------------------------------------------------
    // Called by the managing Lane!
    public void UpdatePosition(Vector3 newPosition, bool visible)
    {
        sprite.enabled = visible;
        if (visible) {
            this.transform.position = newPosition;
        }
    }
    // ------------------------------------------------------------
    public void Destroy()
    {
        Debug.Log("Note destroyed lol");
        Destroy(gameObject);
    }
}
