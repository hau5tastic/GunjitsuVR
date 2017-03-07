// ------------------------------------------------------------
// Track - Manages a number of Lanes. Also contains settings for the current song (BPM, scroll speed, etc.) 
// ------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
// ------------------------------------------------------------
public class Track : MonoBehaviour
{
    // ------------------------------------------------------------
    //Track Fields 
    int bpm = 120;
    int scrollSpeed = 10; //in unity units per second
    int startOffset = 1000; // in milliseconds
    int gridDivisionsIndex = 0;
    string trackName = "uninitialized"; //same as filename
    string songName = "uninitialized";
    // ------------------------------------------------------------
    //Constants
    public static int[] DIVISIONS = { 1, 2, 3, 4, 6, 8, 12, 16 }; //how to make this constant?
    const int MINIMUM_BPM = 1;
    const int MINIMUM_OFFSET = 0;
    // ------------------------------------------------------------
    //Dependencies
    [SerializeField]
    InputField bpmInputField;
    [SerializeField]
    Slider divisionsSlider;
    [SerializeField]
    Lane[] lanes;
    [SerializeField]
    InputField offsetInputField;
    [SerializeField]
    SongController songController;
    [SerializeField]
    Text trackInfoText;
    // ------------------------------------------------------------
    void Start()
    {
        if (lanes == null || lanes.Length != 8 || !bpmInputField
            || !offsetInputField || !divisionsSlider || !trackInfoText
            || !songController)
        {
            Util.Quit("Track is not initialized properly, you shit!");
        }
        trackName = TrackInfo.TrackName;
        songName = TrackInfo.SongName;

        if (TrackInfo.LOAD_TRACK)
        {
            readFromFile();
        } else
        { //create empty track

            Debug.Log("TrackData Check: " + bpm + ", " + ScrollSpeed + ", " + StartOffset);
            songController.loadSong(songName);
        }

        UpdateUI();
        NotifyLanesOnChange();
    }
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    // Updating Fields/UI/Objects
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    public void UpdateBPM()
    {
        bpm = int.Parse(bpmInputField.text);
        if (bpm < MINIMUM_BPM)
        {
            bpm = MINIMUM_BPM;
            bpmInputField.text = "" + MINIMUM_BPM;
        }
        NotifyLanesOnChange();
    }
    // ------------------------------------------------------------
    public void UpdateOffset()
    {
        startOffset = int.Parse(offsetInputField.text);
        if (startOffset < MINIMUM_OFFSET)
        {
            startOffset = MINIMUM_OFFSET;
            offsetInputField.text = "" + MINIMUM_OFFSET;
        }
        NotifyLanesOnChange();
    }
    // ------------------------------------------------------------
    public void UpdateGridDivisions()
    {
        gridDivisionsIndex = (int)divisionsSlider.value;
        NotifyLanesOnChange();
    }
    // ------------------------------------------------------------
    //Use this when loading a Track
    void SetTrack(int newBPM, int newOffset, int newScrollSpeed, string newSongName)
    {
        bpm = newBPM;
        startOffset = newOffset;
        scrollSpeed = newScrollSpeed;
        songName = newSongName;
        gridDivisionsIndex = 0; //back to default

        //Update UI and Grid
        UpdateUI();
        NotifyLanesOnChange();
    }
    // ------------------------------------------------------------
    void UpdateUI()
    {
        bpmInputField.text = "" + bpm;
        offsetInputField.text = "" + startOffset;
        divisionsSlider.value = gridDivisionsIndex; //default
        trackInfoText.text = trackName + "\n" + songName;
    }
    // ------------------------------------------------------------
    void NotifyLanesOnChange()
    {
        foreach(Lane lane in lanes)
        {
            lane.calculateGrid();
        }
    }

    // ------------------------------------------------------------
    // ------------------------------------------------------------
    // Properties (Getters)
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    public int BPM
    {
        get { return bpm; }
    }
    // ------------------------------------------------------------
    public int StartOffset
    {
        get { return startOffset; }
    }
    // ------------------------------------------------------------
    public int ScrollSpeed
    {
        get { return scrollSpeed; }
    }
    // ------------------------------------------------------------
    //Returns the actual number of grid divisions; not the index
    public int GridDivisions
    {
        get { return DIVISIONS[gridDivisionsIndex]; }
    }
    // ------------------------------------------------------------
    public SongController Song
    {
        get { return songController; }
    }
    // ------------------------------------------------------------
    // File I/O and Quit
    // ------------------------------------------------------------
    public void readFromFile()
    {
        if (File.Exists(getFileName()))
        {
            BinaryReader file =
                new BinaryReader(File.Open(getFileName(), FileMode.Open));
            try
            {
                float versionNumber = file.ReadSingle();
                if (versionNumber != Util.VERSION_NUMBER)
                {
                    throw new System.Exception("Editor is incompatible with this track version: " + versionNumber);
                }

                songName = file.ReadString(); 
                bpm = file.ReadInt32();
                startOffset = file.ReadInt32();
                scrollSpeed = file.ReadInt32();
                Debug.Log("Track/Loading Data: " + songName + ", " + bpm + " " + startOffset + " " + scrollSpeed);
                foreach (Lane lane in lanes)
                {
                    lane.readFromFile(file);
                }
            }

            catch (EndOfStreamException e)
            {
                Debug.Log("You made me read an empty/corrupted file, douchebag! " + e.Message);
            }
            //catch (System.Exception e)
            //{
            //    Debug.Log(e.Message);
            //}
            finally
            {
                file.Close();
            }
        }
        else
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
            file.Write(Util.VERSION_NUMBER);
            file.Write(songName);
            file.Write(bpm);
            file.Write(startOffset);
            file.Write(scrollSpeed);
            foreach (Lane lane in lanes)
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
        SceneManager.LoadScene(Util.SCENE_OPEN);
    }
    // ------------------------------------------------------------
    public string getFileName()
    {
        return Util.TRACK_DIR_PREFIX + trackName;//+ FILE_EXTENSION;
    }
    // ------------------------------------------------------------
}
