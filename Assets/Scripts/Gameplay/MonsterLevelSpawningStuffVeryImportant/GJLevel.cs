using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine.SceneManagement;

namespace GJScore
{
    public enum GJAccuracy
    {
        PERFECT,
        GREAT,
        GOOD,
        OK
    }
}

public class GJLevel : MonoBehaviour {
    [Header("UI Prefabs / Scripts")]
    [SerializeField]
    GameObject worldCanvas;
    [SerializeField]
    GameObject introMenu;
    [SerializeField]
    GameObject levelMenu;
    [SerializeField]
    VictoryMenu victoryMenu;
    [SerializeField]
    VictoryMenu defeatMenu;

    public float closingDelay;
    float closingTime;

    [Header("Game Settings")]
    public float spawnRange;
    public float killRange;
    public float killRangeOffset;
    public float overridePlaySpeed;
    public float manualOffset;
    public bool autoPlaySong = false;

    [Header("Score Accuracy")]
    public float perfect = 0.125f;
    public float great = 0.25f;
    public float good = 0.5f;
    public float ok = 1.0f;

    [Header("Score Points")]
    public int perfectScore = 1000;
    public int greatScore = 500;
    public int goodScore = 250;
    public int okScore = 100;

    [Header("Score Colors")]
    public Color perfectColor = Color.green;
    public Color greatColor = Color.blue;
    public Color goodColor = Color.yellow;
    public Color okColor = Color.red;

    [Header("Display Time")]
    public float displayTime = 1.5f;

    public enum TrackDifficulty { NONE, BEGINNER }
    [Header("Track File")]
    public string customTrackName;
    // [Range(0.0f, 1.0f)]
    // public float musicStartTime;
    public float trackStartOffset;
    public TrackDifficulty trackDifficulty = TrackDifficulty.NONE;
    float elapsedTime;

    [SerializeField]
    bool isPlaying = false;
    [SerializeField]
    bool trackEnded = false;
    public bool isPaused = false; //fuck this code :{)
    public bool dontSpawnAnotherDefeatVictoryScreen = false; //forkkk

    [Header("Level Spawners")]
    public GJMonsterSpawner[] monsterSpawners;
    int[] notePointers;
    int notesSpawned;

    [Header("Level Monster Types")]
    public GameObject[] monsterPrefabs;

    [Header("Player Properties")]
    public Slider synchroSlider;
    public static int hitCount = 0;
    public static int fortune = 12000;
    public static float synchronization = 100; // HP
    static float regenerationValue = 1; // per second

    GJSongTrack currentTrack;

    void SetGameSettings()
    {
        GameSettings.spawnRange = spawnRange;
        GameSettings.killRange = killRange;

        GameSettings.perfect = perfect;
        GameSettings.great = great;
        GameSettings.good = good;
        GameSettings.ok = ok;

        GameSettings.perfectScore = perfectScore;
        GameSettings.greatScore = greatScore;
        GameSettings.goodScore = goodScore;
        GameSettings.okScore = okScore;

        GameSettings.perfectColor = perfectColor;
        GameSettings.greatColor = greatColor;
        GameSettings.goodColor = goodColor;
        GameSettings.okColor = okColor;

        GameSettings.displayTime = displayTime;

        GameSettings.autoPlaySong = autoPlaySong;
    }

    void Awake() {
        SetGameSettings();
        worldCanvas.SetActive(true);
    }

    void Start() {
        //GJLevel now waits for another class to activate it.
    }

    public void StartLevel()
    {
        loadTrackName();
        readFromFile();
        loadSong();
        currentTrack.countNotes();
        currentTrack.sortNotes();
        closingTime = closingDelay;
    }

	void Update () {

        if (synchronization < 100) {
            if (synchronization <= 0) {
                synchronization = 0;
                defeatEnd();
            }
        }  else if (synchronization > 100) {
            synchronization = 100;
        }

        //synchroSlider.value = synchronization;
        HealthIndicator.reference.SetHealth(synchronization);

        if (!isPlaying) return;
        synchronization += regenerationValue * Time.deltaTime;
        if (trackEnded)
        {
            victoryEnd();
        }


        if (elapsedTime >= 0) {
            if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
            elapsedTime = GetComponent<AudioSource>().time;
        } else {
            elapsedTime += Time.deltaTime;
        }

        if (notesSpawned >= currentTrack.noteCount) {
            // Track is Over.. Do Something...
            closingTime -= Time.deltaTime;
            if (closingTime <= 0) {
                trackEnded = true;
            }
        }

        /// Each spawner has a corresponding index pointer that points to the current note in that lane.
        /// this is so that every update it will only look at those values instead of the whole list, (assuming that the notes are already sorted)
        /// 
        // We check the number of pointers we have.. this should be == to the number of spawners
        for (int i = 0; i < notePointers.Length; ++i) {

            // for each those pointers, we get the corresponding spawner note list (each spawner has its own list of notes)
            float[] spawnerNotes = currentTrack.notes[i];
            int[] spawnerNoteTypes = currentTrack.noteTypes[i];

            // Next, we need to get the specific note from a specific spawner from the spawners current note index,
            // but first, need to check if the pointer is valid.
            // If the spawner's pointer index value is greater than the length of that spawner's notes, it is of course -- invalid.
            if (notePointers[i] >= spawnerNotes.Length || spawnerNotes.Length <= 0) {
                continue; // Move on to the next spawner
            } else {
                // Debug.Log("index: " + notePointers[i] + "= " + spawnerNotes[notePointers[i]]);
                float currentNote = spawnerNotes[notePointers[i]];
                // Debug.Log("currentNote: " + currentNote + " elapsedTime: " + elapsedTime);
                if (currentNote + GameSettings.timeOffset <= elapsedTime) {
                    // SpawnMonsterOnSpawner(Random.Range(0, 8), 0); // The random version
                    SpawnMonsterOnSpawner(i, spawnerNoteTypes[notePointers[i]], spawnerNoteTypes[notePointers[i]]); // The legit version
                    notePointers[i]++;
                    notesSpawned++;
                }
                
            }
           
        }

	}

    void SpawnMonsterOnSpawner(int spawnerIndex, int monsterTypeIndex, int spawnerNoteType) {
        monsterSpawners[spawnerIndex].Spawn(monsterPrefabs[monsterTypeIndex], spawnerNoteType);
    }

    public void readFromFile() {
        currentTrack = new GJSongTrack();
        
        Debug.Log("Reading the track from: " + getFileName());
        if (File.Exists(getFileName())) {
            BinaryReader file =
                new BinaryReader(File.Open(getFileName(), FileMode.Open));
            try {
                // Version Number
                float versionNumber = file.ReadSingle();
                if (versionNumber != Util.VERSION_NUMBER) {
                     throw new System.Exception("Editor is incompatible with this track version: " + versionNumber);
                }
                
                currentTrack.songName = file.ReadString();
                TrackInfo.SongName = currentTrack.songName;
                currentTrack.bpm = file.ReadInt32();
                currentTrack.startOffset = file.ReadInt32();
                currentTrack.scrollSpeed = file.ReadInt32();

                GameSettings.playSpeed = currentTrack.scrollSpeed;

                if (overridePlaySpeed > 0) GameSettings.playSpeed = overridePlaySpeed;


                GameSettings.timeOffset = -((GameSettings.spawnRange - GameSettings.killRange) / (GameSettings.playSpeed));
                GameSettings.timeOffset += currentTrack.startOffset / 1000f;
                // Debug.Log("Editor Offset: " + currentTrack.startOffset);
                if (manualOffset > 0) {
                    GameSettings.timeOffset = manualOffset;
                }

                currentTrack.notes = new List<float[]>();
                currentTrack.noteTypes = new List<int[]>();
              
                // For each spawner
                foreach (GJMonsterSpawner ms in monsterSpawners ) {
                    int noteCount = file.ReadInt32(); // read the number of notes of 'this' spawner
                    float[] spawnerNotes = new float[noteCount];
                    int[] spawnerNoteTypes = new int[noteCount];

                    // Load that many notes
                    for (int i = 0; i < noteCount; i++) {
                        spawnerNotes[i] = file.ReadSingle();
                        spawnerNoteTypes[i] = file.ReadInt32();
                        //Debug.Log(spawnerNoteTypes[i]);
                    }

                    currentTrack.notes.Add(spawnerNotes);
                    currentTrack.noteTypes.Add(spawnerNoteTypes);
                }
            }

            catch (EndOfStreamException e) {
                Debug.Log("File Empty or Corrupted" + e.Message);
            }
            catch (System.Exception e) {
                Debug.Log(e.Message);
            }
            finally {
                file.Close();
            }
        }
        else {
            Debug.Log("File does not exist.");
        }
        
    }
    public void loadTrackName()
    {
        customTrackName = TrackInfo.TrackName;
    }

    public void loadSong()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        Util.loadAudioClip(audioSource, TrackInfo.SongName);
    }
    
    public string getFileName() {
        //string difficultyTag = "";
        //switch (trackDifficulty) {
        //    case TrackDifficulty.BEGINNER:
        //    // case TrackDifficulty.INTERMEDIATE:
        //    // case TrackDifficulty.EXPERT:
        //        difficultyTag = " [BEGINNER]";
        //        break;
        //    default:
        //        break;

        //}

        //if (customTrackName == "" ) customTrackName = GetComponent<AudioSource>().clip.name + difficultyTag;
        return Util.TRACK_DIR_PREFIX + customTrackName; 
    }

    void Reset() {
        // GetComponent<AudioSource>().time = musicStartTime * GetComponent<AudioSource>().clip.length;
        // elapsedTime = musicStartTime * GetComponent<AudioSource>().clip.length;
        elapsedTime = -trackStartOffset;

        trackEnded = false;
        closingTime = closingDelay;
        notesSpawned = hitCount = 0;
        isPlaying = true;
        Time.timeScale = 1f;
        RenderSettings.ambientIntensity = 1f; // temporary paused indicator
        GetComponent<AudioSource>().volume = 1f;
        GetComponent<AudioSource>().Stop();
        synchronization = 100;
        fortune = 0;
        ScoreText.reference.Reset();

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
        foreach (GameObject go in monsters) {
            Destroy(go);
        }


        notePointers = new int[currentTrack.notes.Count];

        for (int i = 0; i < currentTrack.notes.Count; ++i) {
            notePointers[i] = 0;
        }

        GJGuideReticle.rReticleQ.Clear();
        GJGuideReticle.lReticleQ.Clear();

        victoryMenu.GetComponent<AudioSource>().Stop();
        defeatMenu.GetComponent<AudioSource>().Stop();
    }


    // UI Methods

    public void ReturnToMain() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PauseGame() {

        Time.timeScale = 0f;
        if (GetComponent<AudioSource>().isPlaying) {
            GetComponent<AudioSource>().Pause();
        }

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
        foreach (GameObject go in monsters) {
            go.GetComponent<GJMonster>().paused = true;
        }

        RenderSettings.ambientIntensity = 0f;
        isPlaying = false;
        isPaused = true;
        levelMenu.SetActive(true);
    }

    public void ResumeGame() {
        RenderSettings.ambientIntensity = 1f;
        Time.timeScale = 1f;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
        foreach (GameObject go in monsters) {
            go.GetComponent<GJMonster>().paused = false;
        }
        GetComponent<AudioSource>().UnPause();
        isPlaying = true;
        isPaused = false;
        levelMenu.SetActive(false);
    }

    public void ShowIntro()
    {
        introMenu.SetActive(true); // ??
        levelMenu.SetActive(false);
        victoryMenu.gameObject.SetActive(false);
        defeatMenu.gameObject.SetActive(false);
        dontSpawnAnotherDefeatVictoryScreen = true;
    }

    public void RestartGame() {
        introMenu.SetActive(false);
        levelMenu.SetActive(false);
        victoryMenu.gameObject.SetActive(false);
        defeatMenu.gameObject.SetActive(false);
        Reset();
        dontSpawnAnotherDefeatVictoryScreen = false;
    }

    public void victoryEnd() {
        GetComponent<AudioSource>().volume -= 0.01f;
        if (GetComponent<AudioSource>().volume <= 0 && !dontSpawnAnotherDefeatVictoryScreen) {
            victoryMenu.gameObject.SetActive(true);
            victoryMenu.newAccuracy = (float)hitCount / notesSpawned * 100.0f;
            victoryMenu.newFortune = fortune;
        }
    }

    public void defeatEnd() {
        isPlaying = false;
        GetComponent<AudioSource>().volume -= 0.01f;
        if (GetComponent<AudioSource>().volume <= 0 && !dontSpawnAnotherDefeatVictoryScreen) { //WTF??? <----
            defeatMenu.gameObject.SetActive(true);
            defeatMenu.newAccuracy = (float)hitCount / notesSpawned * 100.0f;
            defeatMenu.newFortune = fortune;

            if (GetComponent<AudioSource>().isPlaying) {
                GetComponent<AudioSource>().Pause();
            }

            GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
            foreach (GameObject go in monsters) {
                go.GetComponent<GJMonster>().paused = true;
            }
            //RenderSettings.ambientIntensity = 0f;
        }
    }
}
