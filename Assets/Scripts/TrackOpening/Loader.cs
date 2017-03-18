// ------------------------------------------------------------
// Loader - Loads and handles Tracks and Songs.
// ------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
// ------------------------------------------------------------
public class Loader : MonoBehaviour
{
    // ------------------------------------------------------------
    //Dependencies
    [SerializeField]
    Text[] trackTexts;
    [SerializeField]
    Text[] songTexts;
    [SerializeField]
    InputField trackNameInput;
    [SerializeField]
    Slider trackNameSlider;
    [SerializeField]
    Slider songNameSlider;
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    //Internals
    List<string> trackNames;
    List<string> songNames;
    const int CURSOR_TRACK = 0;
    const int CURSOR_SONG = 1;
    const int CURSOR_NUM = 2;
    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        if (trackTexts == null || songTexts == null 
            || !songNameSlider || !trackNameSlider || !trackNameInput)
        {
            Util.Quit("Loader.cs/Start() - Loader is not initialized properly!");
            return;
        }
        InitLists();
        InitSliders();
        UpdateListTexts();
    }
    // ------------------------------------------------------------
    void InitLists()
    {
        trackNames = Util.getTrackNames();
        songNames = Util.getSongNames();
    }
    // ------------------------------------------------------------
    void InitSliders()
    {
        //Initialize Sliders
        int sliderValue = songNames.Count - 1;
        if (sliderValue < 0) sliderValue = 0;
        songNameSlider.maxValue = sliderValue;
        
        sliderValue = trackNames.Count - 1;
        if (sliderValue < 0) sliderValue = 0;
        trackNameSlider.maxValue = sliderValue;

    }
    // ------------------------------------------------------------
   
    // ------------------------------------------------------------
    public void UpdateListTexts()
    {
        UpdateListText(trackTexts, trackNames, (int)trackNameSlider.value);
        UpdateListText(songTexts, songNames, (int)songNameSlider.value);
    }
    // ------------------------------------------------------------
    private void UpdateListText(Text[] texts, List<string> names, int cursor)
    {
        int limit = texts.Length;
        if (limit > names.Count) { limit = names.Count; }
        for (int i = 0; i < limit; i++)
        {
            texts[i].color = Color.white;
            texts[i].GetComponent<Outline>().effectColor = Color.black;

            int listMove = cursor - 4;
            if (listMove < 0) { listMove = 0; }
            texts[i].text = names[i + listMove];
        }


        for (int i = limit; i < texts.Length; i++)
        {
            texts[i].text = "-";
        }

        int index = cursor >= texts.Length ? texts.Length - 1 : cursor;
        if (index >= 0)
        {
            texts[index].color = Color.black;
            texts[index].GetComponent<Outline>().effectColor = Color.white;
        }
    }
    // ------------------------------------------------------------
    public void loadCurrentTrack()
    {
        int index = (int)trackNameSlider.value;
        string dir = Util.TRACK_DIR_PREFIX + trackNames[index];

        if (File.Exists(dir))
        {
            BinaryReader file =
                new BinaryReader(File.Open(dir, FileMode.Open));
            try
            {
                float versionNumber = file.ReadSingle();
                if (versionNumber != Util.VERSION_NUMBER)
                {
                    throw new System.Exception("Loader.cs/loadCurrentTrack() - Editor is incompatible with this track version: " + versionNumber);
                }

                TrackInfo.FullFilePath = dir;
                TrackInfo.TrackName = trackNames[index];
                TrackInfo.SongName = file.ReadString();
                TrackInfo.LOAD_TRACK = true;
            }
            catch (EndOfStreamException e)
            {
                Debug.Log("Loader.cs/loadCurrentTrack() - Unexpected end of file. " + e.Message);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            finally
            {
                file.Close();
                SceneManager.LoadScene(Util.SCENE_EDIT);
            }
        }
        else
        {
            Debug.Log("Loader.cs/loadCurrentTrack() - File does not exist.");
        }
    }
    // ------------------------------------------------------------
    public void createNewTrack()
    {
        Debug.Log("Loader.cs/createNewTrack() - Creating new track.");
        TrackInfo.LOAD_TRACK = false;
        TrackInfo.TrackName = trackNameInput.text + Util.TRACK_FILE_EXTENSION;
        int songNameLength = songNames[(int)songNameSlider.value].Length;
        TrackInfo.SongName = songNames[(int)songNameSlider.value].Remove(songNameLength - 4); //Remove Extension
        SceneManager.LoadScene(Util.SCENE_EDIT);
    }
    // ------------------------------------------------------------
    public void quit()
    {
        Util.Quit();
    }
    // ------------------------------------------------------------
}
