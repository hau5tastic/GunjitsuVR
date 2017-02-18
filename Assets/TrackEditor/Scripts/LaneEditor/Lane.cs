// ------------------------------------------------------------
// Lane - Manages a collection of Notes.
// ------------------------------------------------------------
//TODO Editor FindObjectOfType once instead of per frame 
//TODO camera/track efficiencies, redundant checks per frame
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Lane : MonoBehaviour
{
    // ------------------------------------------------------------
    // From start of lane to end
    [SerializeField]
    Vector3 direction;

    // Sideways lane direction
    [SerializeField]
    Vector3 sideDirection;

    [SerializeField]
    float width = 0.5f;

    [SerializeField]
    GameObject notePrefab;

    [SerializeField]
    LineRenderer lineRenderer;

    // ------------------------------------------------------------
    List<Note> notes;

    SongController song;

    Track track;

    Vector3[] gridPoints;
    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        //Let's get the song controller and track.
        song = FindObjectOfType<SongController>();
        track = FindObjectOfType<Track>();

        //Did we fuck up?
        if (direction.magnitude == 0 || notePrefab == null
            || song == null || lineRenderer == null || track == null)
        {
            Debug.LogError("Lane is not initialized properly, you shit!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        //Let's instantiate Notes!
        notes = new List<Note>();

        //Let's ensure the direction is normalized.
        direction.Normalize();

        recalculateDrawGrid();

        //
        if (FindObjectOfType<Track>().trackMode != Track.TRACK_MODE.EDITING) { lineRenderer.enabled = false; }
    }
    // ------------------------------------------------------------
    //
    void createAndAddNote(float initialPosition)
    {
        GameObject tempGO = Instantiate(notePrefab, this.transform);
        Note tempNote = tempGO.GetComponent<Note>();
        tempNote.initialPosition = initialPosition;
        notes.Add(tempNote);
    }

    // ------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //We calculate the song's current position, and every notes's new position.
        Vector3 scrollSpeedOffset = direction * track.getScrollSpeed();
        Vector3 gridStartOffset = direction * track.getStartOffset() / 1000.0f;
        Vector3 songPosition = song.Time() * -scrollSpeedOffset;
        foreach(Note note in notes)
        {
            Vector3 initialPosition = note.initialPosition * scrollSpeedOffset;
            Vector3 newPosition = this.transform.position + songPosition
                + initialPosition + gridStartOffset;
            note.UpdatePosition(newPosition, song.Time() < note.initialPosition + track.getStartOffset()/1000);
        }

        //TODO make more efficient
        recalculateDrawGrid();
        HandleNoteEditing();

    }
    // ------------------------------------------------------------
    void HandleNoteEditing()
    {
        //Make sure we only draw grid in EDIT mode
        if (FindObjectOfType<Track>().trackMode != Track.TRACK_MODE.EDITING) { return; }

        //The actual mouse detection and handling
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = FindObjectOfType<Camera>();

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Respond to only clicks on this lane. Uses some janky Collider/Camera tricks.
                if (hit.transform.gameObject != this.gameObject) return;

                // Add or delete notes
                Note note = doesNoteExistWithinGrid(getClosestGridPoint(hit.point) + new Vector3(transform.position.x,0,0));
                if (note == null) //note does not exist yo
                {
                    //TODO maybe not use direction? Or keep this only for EDITOR ONLYY
                    float trueGrid = (getClosestGridPoint(hit.point).z - track.getStartOffset() / 1000.0f) / track.getScrollSpeed() + song.Time();
                    Debug.Log("Note creation.");
                    createAndAddNote(trueGrid);

                } else {
                    Debug.Log("Note deletion v1.");
                    notes.Remove(note);
                    Destroy(note.gameObject);
                }
            }
        }
    }
    // ------------------------------------------------------------
    public void removeNote(Note note)
    {
        Debug.Log("Note deletion v2.");
        notes.Remove(note);
        Destroy(note.gameObject);
    }
    // ------------------------------------------------------------
    // Calculates the relevant points of the grid, and draws it.
    public void recalculateDrawGrid()
    {
        //Make sure we only draw grid in EDIT mode
        if (FindObjectOfType<Track>().trackMode != Track.TRACK_MODE.EDITING) { return; }

        //Setup line renderer and grid drawing variables
        lineRenderer.numPositions = (int)(track.getBPM() * (song.Length() / 60)) * track.getGridDivisions() + 3;
        Vector3 lineOffsetSide = sideDirection.normalized * width; //the sides of the lane
        Vector3 lineOffsetForward = direction.normalized; //across the lane
        float gridlineOffset = song.Length() / (lineRenderer.numPositions - 5) * track.getScrollSpeed() * 2; //Distance between grid lines!
        Vector3 songStartOffset = lineOffsetForward * track.getStartOffset() / 1000.0f;
        int accm = 1; //Used below to create a square wave pattern

        //TODO FIX inefficiencies; these are already calculate in Update!!
        Vector3 scrollSpeedOffset = direction * track.getScrollSpeed();
        Vector3 songPosition = song.Time() * -scrollSpeedOffset;

        //Generating points for line renderer
        for (int i = 0; i < lineRenderer.numPositions - 3; i++)
        {
            lineRenderer.SetPosition(i,
                transform.position
                + lineOffsetSide 
                + lineOffsetForward * (i / 2) * gridlineOffset
                + songStartOffset 
                + songPosition
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
            gridPoints[i].x = 0; //TODO cheating, dont code like this
        }

        //Draw final 3 outer grid lines!! 
        lineRenderer.SetPosition(lineRenderer.numPositions - 3,
            transform.position + lineOffsetSide
            + songStartOffset + songPosition);
        lineRenderer.SetPosition(lineRenderer.numPositions - 2,
            transform.position - lineOffsetSide
           + songStartOffset + songPosition);
        lineRenderer.SetPosition(lineRenderer.numPositions - 1,
            lineRenderer.GetPosition(lineRenderer.numPositions - 5));
    }
    // -------------------------------------------------------------
    // Returns the grid point closed to the given Vector3
    // TODO: Efficiency can be improved because the gridpoints are sorted.
    Vector3 getClosestGridPoint(Vector3 point)
    {
        float min = Vector3.Distance(point, gridPoints[0]);
        int index = 0;

        for (int i = 1; i < gridPoints.Length;i++)
        {
            float dist = Vector3.Distance(point, gridPoints[i]);
            if (dist < min)
            {
                index = i;
                min = dist;
            }
        }
        return gridPoints[index];
    }
    // -------------------------------------------------------------
    // Returns a note that corresponds to the given point, only if the note exists
    Note doesNoteExistWithinGrid(Vector3 point)
    {
        foreach(Note note in notes)
        {
            //if (Vector3.Distance(note.transform.position,point) <= 0.1f)
            //Debug.Log("CMP: " + note.transform.position + ", " + point);
            if (note.transform.position == point)
            {
                return note;
            }
        }
        return null;
    }
    // -------------------------------------------------------------
    public void readFromFile(BinaryReader file)
    {
        notes = new List<Note>();
        int noteCount = file.ReadInt32();
        for (int i = 0; i < noteCount; i++)
        {
            createAndAddNote(file.ReadSingle());
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
