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
        if (trackTexts == null || songTexts == null)
        {
            Util.Quit("Loader is not initialized properly!!!");
        }

        trackNames = new List<string>();
        songNames = new List<string>();
        cursors = new int[CURSOR_NUM];

        var dir = new DirectoryInfo(Util.TRACK_DIR_PREFIX);
        var files = dir.GetFiles("*" + Util.TRACK_FILE_EXTENSION);
        foreach (FileInfo file in files)
        {
            trackNames.Add(file.Name);
        }

        dir = new DirectoryInfo(Util.SONG_DIR_PREFIX); //TODO Handle multiple file exts.
        files = dir.GetFiles("*.wav");
        foreach (FileInfo file in files)
        {
            songNames.Add(file.Name.Remove(file.Name.Length-4));
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            loadCurrentTrack();
        }
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
                    throw new System.Exception("Editor is incompatible with this track version: " + versionNumber);
                }

                TrackInfo.FullFilePath = dir;
                TrackInfo.TrackName = trackNames[cursors[CURSOR_TRACK]];
                TrackInfo.SongName = file.ReadString();
                TrackInfo.LOAD_TRACK = true;
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
                SceneManager.LoadScene(Util.SCENE_EDIT);
            }
        }
        else
        {
            Debug.Log("File does not exist, fool!");
        }
    }
    // ------------------------------------------------------------
    public void createNewTrack()
    {
        Debug.Log("Loader/ Creating new track.");
        TrackInfo.LOAD_TRACK = false;
        TrackInfo.TrackName = trackNameInput.text + Util.TRACK_FILE_EXTENSION;
        TrackInfo.SongName = songNames[cursors[CURSOR_SONG]];
        SceneManager.LoadScene(Util.SCENE_EDIT);
    }
    // ------------------------------------------------------------
    public void quit()
    {
        Util.Quit();
    }
    // ------------------------------------------------------------
}
