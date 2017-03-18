// ------------------------------------------------------------
// Note - A single note within a Lane. 
// ------------------------------------------------------------
using UnityEngine;
using System;
// ------------------------------------------------------------
[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour, IComparable
{
    public enum NOTE_TYPE { LEFT = 0, RIGHT = 1 };
    // ------------------------------------------------------------
    // State
    public float initialPosition { get; set; }
    NOTE_TYPE noteType;
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
        if (visible)
        {
            this.transform.position = newPosition;
        }
    }
    // ------------------------------------------------------------
    public NOTE_TYPE getNoteType()
    {
        return noteType;
    }
    // ------------------------------------------------------------
    public void setNoteType(NOTE_TYPE newNoteType)
    {
        noteType = newNoteType;

        //change color
        Color newColor = Color.white;
        switch (noteType)
        {
            case NOTE_TYPE.LEFT:
                newColor = new Color(0, 0.75f, 1.0f);
                break;
            case NOTE_TYPE.RIGHT:
                newColor = Color.red;
                break;
            default:
                Util.Quit("Note.cs/setNoteType() - Unexpected note type.");
                break;
        }

        sprite.color = newColor;
    }
    // ------------------------------------------------------------
    public int CompareTo(object obj)
    {
        if (obj == null) { return 1; }

        Note otherNote = obj as Note;

        if (otherNote != null)
        {
            return initialPosition.CompareTo(otherNote.initialPosition);
        }
        else
        {
            throw new ArgumentException("Object is not a Temperature");
        }
    }
    // ------------------------------------------------------------
}
