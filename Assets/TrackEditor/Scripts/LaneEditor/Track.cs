// ------------------------------------------------------------
// Track - Manages a number of Lanes. Also contains settings for the current song (BPM, scroll speed, etc.) 
// ------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class Track : MonoBehaviour
{
    // ------------------------------------------------------------
    //Track fields 
    int bpm = 120;
    int scrollSpeed = 5;
    int startOffset = 1000; // in milliseconds
    int gridDivisions = 1; // 1 ~ 16 recommended [Used for track editing]
    string trackName = "Nameless";

    public TRACK_MODE trackMode = TRACK_MODE.GAMEPLAY;

    // ------------------------------------------------------------
    //Constants
    int[] DIVISIONS = { 1, 2, 3, 4, 6, 8, 12, 16 }; //hard coded cause money is tight
    const string FILEPATH_PREFIX = "Assets/TrackEditor/Tracks/";
    const string FILE_EXTENSION = ".gj";
    const float VERSION_NUMBER = 1f;
    public enum TRACK_MODE { GAMEPLAY, EDITING };
    // ------------------------------------------------------------
    //Internals
    [SerializeField]
    Lane[] lanes;

    // ------------------------------------------------------------
    void Start()
    {
        if (lanes == null || lanes.Length != 8)
        {
            Debug.LogError("Track is not initialized properly, you shit!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    // ------------------------------------------------------------
    // Updating track fields
    // ------------------------------------------------------------
    public void UpdateBPM(InputField bpmInputField)
    {
        bpm = int.Parse(bpmInputField.text);
        notifyLanesOnGridChange();
    }

    public void UpdateScrollSpeed(InputField scrollSpeedInputField)
    {
        scrollSpeed = int.Parse(scrollSpeedInputField.text);
        notifyLanesOnGridChange();
    }

    public void UpdateScrollSpeed(int newSpeed)
    {
        scrollSpeed = newSpeed;
        notifyLanesOnGridChange();
    }

    public void UpdateOffset(InputField startOffsetInputField)
    {
        startOffset = int.Parse(startOffsetInputField.text);
        notifyLanesOnGridChange();
    }

    public void UpdateGridDivisions(Dropdown gridDivisionsInputField)
    {
        gridDivisions = DIVISIONS[gridDivisionsInputField.value];
        notifyLanesOnGridChange();
    }

    public void UpdateTrackName(InputField trackNameInputField)
    {
        trackName = trackNameInputField.text;
    }


    void notifyLanesOnGridChange()
    {
        foreach (Lane lane in lanes)
        {
            lane.recalculateDrawGrid();
        }
    }

    // ------------------------------------------------------------
    // Accessing track fields
    // ------------------------------------------------------------
    public int getBPM()
    {
        return bpm;
    }

    public int getScrollSpeed()
    {
        return scrollSpeed;
    }

    public int getStartOffset()
    {
        return startOffset;
    }

    public int getGridDivisions()
    {
        return gridDivisions;
    }
    // ------------------------------------------------------------
    // File I/O and Quit
    // ------------------------------------------------------------
    public void readFromFile()
    {
        if (!EditorUtility.DisplayDialog("Reload this track?",
            "Are you sure you want to load \"" + trackName
            + "\"? Make sure you don't have any unsaved changes, ばか！", "Load", "Do Not load"))
        {
            return;
        }

        Debug.Log("We readin' the track from: " + getFileName());
        if (File.Exists(getFileName()))
        {
            BinaryReader file =
                new BinaryReader(File.Open(getFileName(), FileMode.Open));
            try
            {
                // Version Number
                float versionNumber = file.ReadSingle();
                if (versionNumber != VERSION_NUMBER) {
                    throw new System.Exception("Editor is incompatible with this track version: " + versionNumber);
                }
                // Song name TODO: make use of this.
                string songName = file.ReadString();
                // Bpm
                bpm = file.ReadInt32();
                // StartOffset
                startOffset = file.ReadInt32();
                // Default Scroll Speed
                scrollSpeed = file.ReadInt32();
                // Lane/note data
                foreach (Lane lane in lanes)
                {
                    lane.readFromFile(file);
                }
            }

            catch (EndOfStreamException e)
            {
                Debug.Log("You made me read an empty/corrupted file, douchebag! " + e.Message);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                file.Close();
            }
        } else
        {
            Debug.Log("File does not exist, fool!");
        }
    }
    // ------------------------------------------------------------
    public void writeToFile()
    {
        if (!EditorUtility.DisplayDialog("Save this track?",
            "Are you sure you want to save \"" + trackName
            + "\"?", "Save", "Do Not Save"))
        {
            return;
        }

        Debug.Log("We saving the track to: " + getFileName());
        using (BinaryWriter file =
            new BinaryWriter(File.Open(getFileName(), FileMode.Create)))
        {
            // Track Editor Version
            file.Write(VERSION_NUMBER);
            // Song name 
            file.Write(FindObjectOfType<SongController>().GetComponent<AudioSource>().clip.name);
            // Bpm
            file.Write(bpm);
            // StartOffset
            file.Write(startOffset);
            // Default Scroll Speed
            file.Write(scrollSpeed);
            // Lane/note data
            foreach( Lane lane in lanes)
            {
                lane.writeToFile(file);
            }
        }
    }
    // ------------------------------------------------------------
    public void QuitTrackEditing()
    {
        if (!EditorUtility.DisplayDialog("Quit editing this track?",
            "Are you sure you want to quit editing \"" + trackName
            + "\"? Make sure you have saved your work.", "Quit", "Do Not Quit"))
        {
            return;
        }
        UnityEditor.EditorApplication.isPlaying = false;
    }
    // ------------------------------------------------------------
    public string getFileName()
    {
        return FILEPATH_PREFIX + trackName + FILE_EXTENSION;
    }
}
