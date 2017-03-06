// ------------------------------------------------------------
// TrackInfo - Stores header data on the current track across scenes, globally.
// ------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ------------------------------------------------------------

public class TrackInfo 
{
    public static bool LOAD_TRACK
    {
        get;set;
    }
    // ------------------------------------------------------------
    public static string FullFilePath
    {
        get;set;
    }
    // ------------------------------------------------------------
    public static string TrackName 
    {
        get; set;
    }
    // ------------------------------------------------------------
    public static string SongName
    {
        get; set;
    }
    // ------------------------------------------------------------
}
