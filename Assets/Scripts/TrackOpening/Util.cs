using UnityEngine;

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
    public const float VERSION_NUMBER = 1f;
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
}
