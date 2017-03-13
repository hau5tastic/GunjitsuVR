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
    string[] APPROVED_SONG_EXTENSIONS = { "wav", "mp3" };
    // ------------------------------------------------------------
    //Dependencies
    [SerializeField]
    Text[] trackTexts;

    [SerializeField]
    Text[] songTexts;

    [SerializeField]
    InputField trackNameInput;
    // ------------------------------------------------------------
    //Internals
    List<string> trackNames;
    List<string> songNames;
    const int CURSOR_TRACK = 0;
    const int CURSOR_SONG = 1;
    const int CURSOR_NUM = 2;
    int[] cursors; //0: Track, 1: Song
    int cursorIndex = 0;
    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        trackNames = new List<string>();
        songNames = new List<string>();
        cursors = new int[CURSOR_NUM];
        if (trackTexts == null || songTexts == null)
        {
            Util.Quit("Loader.cs/Start() - Loader is not initialized properly!");
            return;
        }
        LoadTrackAndSongNames();
    }
    // ------------------------------------------------------------
    void LoadTrackAndSongNames()
    {
        AddFilesToList(Util.TRACK_DIR_PREFIX, "*" + Util.TRACK_FILE_EXTENSION, trackNames);
        foreach (string extension in APPROVED_SONG_EXTENSIONS)
        {
            AddFilesToList(Util.SONG_DIR_PREFIX, "*." + extension, songNames);
        }

    }
    // ------------------------------------------------------------
    void AddFilesToList(string directory, string pattern, List<string> list)
    {
        var dir = new DirectoryInfo(directory);
        var files = dir.GetFiles(pattern);
        foreach (FileInfo file in files)
        {
            list.Add(file.Name);
        }
    }
    // ------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)
         || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorIndex = (cursorIndex + 1) % 2;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        { cursors[cursorIndex] -= 1; }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        { cursors[cursorIndex] += 1; }

        if (cursors[cursorIndex] < 0) cursors[cursorIndex] = 0;
        if (cursors[CURSOR_TRACK] > trackNames.Count - 1) cursors[CURSOR_TRACK] = trackNames.Count - 1;
        if (cursors[CURSOR_SONG] > songNames.Count - 1) cursors[CURSOR_SONG] = songNames.Count - 1;
        UpdateListTexts();
    }
    // ------------------------------------------------------------
    public void UpdateListTexts()
    {
        UpdateListText(trackTexts, trackNames, cursors[0]);
        UpdateListText(songTexts, songNames, cursors[1]);
    }
    // ------------------------------------------------------------
    public void UpdateListText(Text[] texts, List<string> names, int cursor)
    {
        int limit = texts.Length;
        if (limit > names.Count) { limit = names.Count; }
        for (int i = 0; i < limit; i++)
        {
            texts[i].color = Color.white;

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
            texts[index].color = Color.blue;
        }
    }
    // ------------------------------------------------------------
    public void loadCurrentTrack()
    {
        int index = cursors[CURSOR_TRACK] >= trackTexts.Length ? trackTexts.Length - 1 : cursors[CURSOR_TRACK];
        string dir = Util.TRACK_DIR_PREFIX + trackNames[cursors[CURSOR_TRACK]];

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
                TrackInfo.TrackName = trackNames[cursors[CURSOR_TRACK]];
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
        int songNameLength = songNames[cursors[CURSOR_SONG]].Length;
        TrackInfo.SongName = songNames[cursors[CURSOR_SONG]].Remove(songNameLength - 4); //Remove Extension
        SceneManager.LoadScene(Util.SCENE_EDIT);
    }
    // ------------------------------------------------------------
    public void quit()
    {
        Util.Quit();
    }
    // ------------------------------------------------------------
}
