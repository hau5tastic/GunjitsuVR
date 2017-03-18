using UnityEngine;

using System.Collections.Generic;
using System.IO;

public class Util
{
    // ------------------------------------------------------------
    //Constants
    public const string TRACK_DIR_PREFIX = "Assets/Resources/Tracks/";
    public const string SONG_DIR_PREFIX = "Assets/Resources/Songs/";
    public const string SONG_PREFIX = "Songs/";
    public const string SCENE_OPEN = "TrackOpen";
    public const string SCENE_EDIT = "TrackEdit";
    public const string TRACK_FILE_EXTENSION = ".gj";
    public const float VERSION_NUMBER = 1.1f;
    // ------------------------------------------------------------
    static readonly string[] APPROVED_SONG_EXTENSIONS = { "wav", "mp3" };
    // ------------------------------------------------------------
    // Exit the game due to an unexpected circumstance.  
    public static void Quit(string logMessage = "")
    {
        if (logMessage != "") { Debug.LogError(logMessage); }
        Debug.Log("Util.cs/Quit() - Exiting game unexpectedly.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // ------------------------------------------------------------
    public static List<string> getTrackNames()
    {
        List<string> trackNames = new List<string>();
        AddFilesToList(Util.TRACK_DIR_PREFIX, "*" + Util.TRACK_FILE_EXTENSION, trackNames);
        return trackNames;
    }
    // ------------------------------------------------------------
    public static List<string> getSongNames()
    {
        List<string> songNames = new List<string>();
        foreach (string extension in APPROVED_SONG_EXTENSIONS)
        {
            AddFilesToList(Util.SONG_DIR_PREFIX, "*." + extension, songNames);
        } 
        return songNames;
    }
    // ------------------------------------------------------------
    public static void AddFilesToList(string directory, string pattern, List<string> list)
    {
        var dir = new DirectoryInfo(directory);
        var files = dir.GetFiles(pattern);
        foreach (FileInfo file in files)
        {
            list.Add(file.Name);
        }
    }
    // ------------------------------------------------------------
    public static void loadAudioClip(AudioSource audioSource, string file)
    {
        if (file == null)
        {
            Util.Quit("Song file could not be found.");
            return;
        }

        audioSource.clip = Resources.Load<AudioClip>(Util.SONG_PREFIX + file);

        Debug.Log("Util.cs/loadAudioClip() - File: " + file);
        Debug.Log("Util.cs/loadAudioClip() - Loaded: " + audioSource.clip.name);
    }
    // -----------------------------------------------------------
}
