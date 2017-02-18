// ------------------------------------------------------------
// Song Controller - exposes methods to manipulate the song's state or current time
// ------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongController : MonoBehaviour
{
    // ------------------------------------------------------------
    [SerializeField]
    AudioSource currentSong;

    //
    [SerializeField]
    Slider songSlider;

    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        Debug.Log("SongController Start---------------------");

        // Let's check if the programmer fucked up
        if (currentSong == null || songSlider == null)
        {
            Debug.LogError("SongController is not initialized properly, you shit!");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        Debug.Log("Song: " + currentSong.clip.name);
        Debug.Log("\tLength: " + currentSong.clip.length + " seconds.");
    }

    // ------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Song Position: " + currentSong.time);

        songSlider.value = currentSong.time / currentSong.clip.length;
    }

    // ------------------------------------------------------------
    public float Time()
    {
        return currentSong.time;
    }

    // ------------------------------------------------------------
    // Unity's pause and play behavior is not ideal; so we write our own higher level
    // functions and expose them instead.
    // ------------------------------------------------------------
    public void Pause()
    {
        if (currentSong.isPlaying)
        {
            currentSong.Pause();
        }
        else
        {
            currentSong.UnPause();
        }
    }

    // ------------------------------------------------------------
    public void Play()
    {
        if (!currentSong.isPlaying)
        {
            currentSong.Play();
        }
        else
        {
            currentSong.Pause();
        }
    }

    // ------------------------------------------------------------
    public void Stop()
    {
        currentSong.Stop();
    }
    
    public float Length()
    {
        return currentSong.clip.length;
    }
    // ------------------------------------------------------------

    // ------------------------------------------------------------
    // percent - must be between 0 and 1
    public void SetSongPosition(float percent)
    {
        currentSong.time = percent * currentSong.clip.length;
    }
}