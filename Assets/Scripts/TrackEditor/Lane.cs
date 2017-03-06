// ------------------------------------------------------------
// Lane - Manages a collection of Notes.
// ------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
// ------------------------------------------------------------
[RequireComponent(typeof(LineRenderer))]
public class Lane : MonoBehaviour
{
    // ------------------------------------------------------------
    //Constants
    const float NOTE_ADD_LENIENCE = 0.5f;
    const float GRID_WIDTH = 0.5f;
    // ------------------------------------------------------------
    //Internals
    LineRenderer lineRenderer;
    [SerializeField]
    Vector3[] gridPoints;
    List<Note> notes;
    Vector3 originalPosition;
    // ------------------------------------------------------------
    //Dependencies
    Track track;
    Transform parentTransform;
    new Camera camera;
    [SerializeField]
    GameObject notePrefab;
    // ------------------------------------------------------------
    void Awake()
    {
        resetNotes();

        //Retrieve objects
        track = FindObjectOfType<Track>();
        lineRenderer = GetComponent<LineRenderer>();
        parentTransform = GetComponentInParent<Transform>();
        camera = FindObjectOfType<Camera>();
        originalPosition = transform.position;
    }
    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        //Validation
        if (!lineRenderer || !track || !parentTransform || !camera)
        {
            Util.Quit("Grid is not initialized properly, you shit!");
        }
    }
    // ------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        handleClick();
        moveLane();
        updateNotes();
    }
    // ------------------------------------------------------------
    // Calculates the relevant points of the grid and adds it to LineRenderer
    public void calculateGrid()
    {
        Debug.Log("Lane/Calculating grid.");

        //Setup line renderer and grid drawing variables
        lineRenderer.numPositions = (int)(track.BPM * (track.Song.Length / 60)) * 2 * track.GridDivisions + 3;

        //Debug.Log("GridCalc/NumPos: " + track.BPM
        //    + ", " + track.Song.Length
        //    + ", " + track.GridDivisions);

        if (lineRenderer.numPositions % 2 == 0) lineRenderer.numPositions += 1; //Make numPosition an odd number; needed to draw the outer grid
        Vector3 lineOffsetSide = Vector3.up * GRID_WIDTH; 
        Vector3 lineOffsetForward = (float)(track.ScrollSpeed) / track.GridDivisions * Vector3.right / (track.BPM /(float)60);
        Vector3 songStartOffset = Vector3.right * track.StartOffset / 100.0f;
        int accm = 1; //Used to create a square wave pattern

        //Generating square wave points for line renderer
        for (int i = 0; i < lineRenderer.numPositions - 3; i++)
        {
            lineRenderer.SetPosition(i,
                originalPosition
                + lineOffsetSide
                + lineOffsetForward * (i / 2) 
                + songStartOffset
            );

            //Handles 'X' offset pattern of 1, 1, -1, -1 and repeat
            if (++accm == 2)
            {
                lineOffsetSide *= -1;
                accm = 0;
            }
        }

        //Saving relevant grid points
        gridPoints = new Vector3[(int)((lineRenderer.numPositions - 3) / 2)];
        for (int i = 0; i < gridPoints.Length; i++)
        {
            gridPoints[i] = lineRenderer.GetPosition(i * 2);
        }

        //Draw final 3 outer grid lines!! 
        lineRenderer.SetPosition(lineRenderer.numPositions - 3,
            originalPosition + lineOffsetSide + songStartOffset);
        lineRenderer.SetPosition(lineRenderer.numPositions - 2,
            originalPosition - lineOffsetSide + songStartOffset);
        lineRenderer.SetPosition(lineRenderer.numPositions - 1,
            lineRenderer.GetPosition(lineRenderer.numPositions - 5));
    }
    // ------------------------------------------------------------
    public void handleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pt = camera.ScreenToWorldPoint(Input.mousePosition);

            if (Mathf.Abs(originalPosition.y - pt.y) > GRID_WIDTH)
            { return; }

            // Handle Note Deletion
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag != "Note") return;
                Note n = hit.transform.GetComponent<Note>();
                notes.Remove(n);
                Destroy(n.gameObject);
                Debug.Log("Note destroyed.");
                return;
            }

            // Handle Note Creation
            for (int i = 0; i < gridPoints.Length; i++)
            {
                if (Mathf.Abs(gridPoints[i].x + this.transform.position.x - pt.x) < NOTE_ADD_LENIENCE)
                {
                    
                    notes.Add(createNote((float)i / track.GridDivisions / ((float)track.BPM / 60)));
                    Debug.Log("Note Created.");
                    return;
                }
            }
        }
    }
    // ------------------------------------------------------------
    public void updateNotes()
    {
        Vector3 scrollSpeedOffset = Vector3.right * track.ScrollSpeed;
        Vector3 gridStartOffset = Vector3.right * track.StartOffset / 100.0f;
        Vector3 songPosition = track.Song.Time * -scrollSpeedOffset;

        foreach (Note note in notes)
        {
            Vector3 noteStartPosition = note.initialPosition * scrollSpeedOffset;
            Vector3 newPosition = originalPosition + songPosition + noteStartPosition + gridStartOffset;
            note.UpdatePosition(newPosition, true/*track.Song.Time / 1000.0f < note.initialPosition*/);
        }
    }
    // ------------------------------------------------------------
    public void moveLane()
    {
        Vector3 songPosition = track.Song.Time * -Vector3.right * 10;
        this.transform.position = songPosition;
    }
    // ------------------------------------------------------------
    Note createNote(float initialPosition)
    {
        GameObject tempGO = Instantiate(notePrefab, this.transform);
        Note tempNote = tempGO.GetComponent<Note>();
        tempNote.initialPosition = initialPosition;
        return tempNote;
    }
    // -------------------------------------------------------------
    public void resetNotes()
    {
        if (notes == null)
        {
            notes = new List<Note>();
            return;
        }

        for (int i = notes.Count - 1; i >= 0; i--)
        {
            notes[i].Destroy();
        }
        notes.Clear();
    }
    // -------------------------------------------------------------
    public void readFromFile(BinaryReader file)
    {
        resetNotes();
        int noteCount = file.ReadInt32();
        for (int i = 0; i < noteCount; i++)
        {
            notes.Add(createNote(file.ReadSingle()));
        }
    }
    // -------------------------------------------------------------
    public void writeToFile(BinaryWriter file)
    {
        file.Write(notes.Count);
        for (int i = 0; i < notes.Count; i++)
        {
            file.Write(notes[i].initialPosition);
        }
    }
    // -------------------------------------------------------------
}