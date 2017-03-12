// ------------------------------------------------------------
// Song Controller - exposes methods to manipulate the song's state or current time
// ------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ------------------------------------------------------------
public class SongController : MonoBehaviour
{
    // ------------------------------------------------------------
    // Dependencies
    [SerializeField]
    AudioSource currentSong;
    [SerializeField]
    Slider songSlider;
    [SerializeField]
    Text songTimeText;
    [SerializeField]
    Slider playbackSpeedSlider;
    // ------------------------------------------------------------
    // Internals
    //songSliderMutable is used to prevent the Slider from updating the song's time on Update().
    bool songSliderMutable = false;
    // ------------------------------------------------------------
    void Awake()
    {
        loadSong(TrackInfo.SongName);
    }
    // ------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        if (!currentSong || !songSlider || !songTimeText)
        {
            Util.Quit("SongController.cs/Start() - Not initialized properly!");
        }

        playbackSpeedSlider.value = 2;
    }
    // ------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Song Position: " + currentSong.time);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        { Time -= 1; }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        { Time += 1; }
        if (Input.GetKeyDown(KeyCode.Space))
        { Play(); }

        songSliderMutable = false;
        songSlider.value = Time / Length;
        songSliderMutable = true;
        UpdateSongTimeText();
    }
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    // Song Control functions (Play/Pause/Stop) 
    // SongController already has builtin functions; but their behavior is not ideal so we expose our own versions instead.
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    public void Pause()
    {
        if (currentSong.isPlaying)
        { currentSong.Pause(); }
        else
        { currentSong.UnPause(); }
    }
    // ------------------------------------------------------------
    public void loadSong(string file)
    {
        currentSong = GetComponent<AudioSource>();
        currentSong.clip = Resources.Load<AudioClip>(Util.SONG_PREFIX + file);

        Debug.Log("SongController.cs/loadSong() - File: " + file);
        Debug.Log("SongController.cs/loadSong() - Loaded: " + currentSong.clip.name);
    }
    // ------------------------------------------------------------
    public void Play()
    {
        if (!currentSong.isPlaying)
        { currentSong.Play(); }
        else
        { currentSong.Pause(); }
    }
    // ------------------------------------------------------------
    public void Stop()
    {
        currentSong.Stop();
    }
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    // Properties 
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    public float Time
    {
        get { return currentSong.time; }
        set
        {
            if (value < 0)
            {
                currentSong.time = 0;
            }
            else if (value > Length)
            {
                currentSong.time = Length;
            }
            else
            {
                currentSong.time = value;
            }
        }
    }
    // ------------------------------------------------------------
    public float Length
    {
        get { return currentSong.clip == null ? 0 : currentSong.clip.length; }
    }
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    // UI Updating
    // ------------------------------------------------------------
    // ------------------------------------------------------------
    public void UpdateSongTimeText()
    {
        int currentMinutes = (int)(Time / 60);
        int currentSeconds = (int)(Time % 60);
        //Ideally, length calculation should only happen once per song. Oh well!
        int lengthMinutes = (int)(Length / 60);
        int lengthSeconds = (int)(Length % 60);

        songTimeText.text =
            currentMinutes + ":"
            + (currentSeconds > 9 ? "" + currentSeconds : "0" + currentSeconds)
            + " / "
            + lengthMinutes + ":"
            + (lengthSeconds > 9 ? "" + lengthSeconds : "0" + lengthSeconds);
    }
    // ------------------------------------------------------------
    public void SetSongPositionFromSlider()
    {
        if (!songSliderMutable) return;
        currentSong.time = songSlider.value * Length;
    }
    // ------------------------------------------------------------
    public void SetPlaybackSpeed()
    {
        if (playbackSpeedSlider.value == 0)
        {
            currentSong.pitch = 0.3f;
        }
        else
        {
            currentSong.pitch = playbackSpeedSlider.value/2;
        }
        
    }
    // ------------------------------------------------------------
}