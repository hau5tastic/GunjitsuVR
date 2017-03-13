using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GJLevel : MonoBehaviour {
    [SerializeField]
    GameObject introMenu;
    [SerializeField]
    GameObject levelMenu;
    [SerializeField]
    VictoryMenu victoryMenu; 

    public float closingDelay;
    float closingTime;

    [Header("Game Settings")]
    public float spawnRange;
    public float killRange;
    public float overridePlaySpeed;
    public float manualOffset;


    [Header("Track File")]
    public string trackName;

    public bool isPlaying = false;
    bool trackEnded = false;

    [Header("Level Spawners")]
    public GJMonsterSpawner[] monsterSpawners;
    int[] notePointers;
    int notesSpawned;

    [Header("Level Monster Types")]
    public GameObject[] monsterPrefabs;

    float elapsedTime;

    [Header("Player Properties")]
    public static int accuracy = 100;
    public static int fortune = 12000;
    // float synchronization; // HP
    // int fortune; // Score

    GJSongTrack currentTrack;

    void Awake() {
        GameSettings.spawnRange = spawnRange;
        GameSettings.killRange = killRange;

    }

    void Start() {

        // Time.timeScale = 0.5f;
        // Time.fixedDeltaTime = 0.02f * Time.timeScale;
        readFromFile();
        currentTrack.countNotes();
        currentTrack.sortNotes();
        closingTime = closingDelay;
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            PauseGame();
        }

        if (!isPlaying) return;
        if (trackEnded) {
            victoryEnd();
        }
        elapsedTime += Time.deltaTime;

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
                    SpawnMonsterOnSpawner(i, 0, spawnerNoteTypes[notePointers[i]]); // The legit version
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

    public string getFileName() {
        return Util.TRACK_DIR_PREFIX + trackName + Util.TRACK_FILE_EXTENSION;
    }

    void Reset() {
        closingTime = closingDelay;
        notesSpawned = 0;
        isPlaying = true;
        Time.timeScale = 1f;
        RenderSettings.ambientIntensity = 1f; // temporary paused indicator
        GetComponent<AudioSource>().volume = 0.5f;
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
        foreach (GameObject go in monsters) {
            Destroy(go);
        }

        
        elapsedTime = 0f;
        notePointers = new int[currentTrack.notes.Count];

        for (int i = 0; i < currentTrack.notes.Count; ++i) {
            notePointers[i] = 0;
        }

        GJGuideReticle.rReticleQ.Clear();
        GJGuideReticle.lReticleQ.Clear();
    }


    // UI Methods

    public void ReturnToMain() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PauseGame() {
        levelMenu.SetActive(true);
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
        levelMenu.SetActive(false);
    }

    public void RestartGame() {
        introMenu.SetActive(false);
        levelMenu.SetActive(false);
        Reset();
    }

    public void victoryEnd() {
        GetComponent<AudioSource>().volume -= 0.01f;
        if (GetComponent<AudioSource>().volume <= 0) {
            victoryMenu.gameObject.SetActive(true);
            victoryMenu.newAccuracy = accuracy;
            victoryMenu.newFortune = fortune;
        }
    }


}
