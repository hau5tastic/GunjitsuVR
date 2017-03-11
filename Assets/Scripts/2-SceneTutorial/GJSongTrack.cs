using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJSongTrack {

    public float versionNumber;
    public string songName;
    public int bpm;
    public int scrollSpeed;
    public int startOffset;
    public List<float[]> notes;
    public int noteCount;


    public void countNotes() {
        noteCount = 0;
        foreach (float[] fA in notes) {
            noteCount += fA.Length;
        }
    }

    public void sortNotes() {
        foreach (float[] fA in notes) {
            System.Array.Sort(fA);
        }
    }
     
}
