// ------------------------------------------------------------
// Note - A single note within a Lane. 
// ------------------------------------------------------------
using UnityEngine;
// ------------------------------------------------------------
[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour
{
    public enum NOTE_TYPE {LEFT=0, RIGHT=1};
    // ------------------------------------------------------------
    // State
    public float initialPosition { get; set; }
    public NOTE_TYPE noteType { get; set; }
    // ------------------------------------------------------------
    // Dependencies 
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
}
