using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    using UnityEditor;
#endif

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

    public static GJLevel instance = null;

    [Header("UI Prefabs / Scripts")]
    public float closingDelay;
    float closingTime;

    [Header("Game Settings")]
    public float spawnRange;
    public float killRange;
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

    [Header("Track File")]


    public float trackStartOffset;
    float elapsedTime;

    [SerializeField]
    bool isInProgress = false;
    [SerializeField]
    bool trackEnded = false;
    public bool isPaused = false;

    [Header("Level Spawners")]
    public GJMonsterSpawner[] monsterSpawners;
    int[] notePointers;
    int notesSpawned;

    [Header("Level Monster Types")]
    public GameObject[] monsterPrefabs;

    [Header("Player Properties")]
    public static int hitCount = 0;
    public static float accuracy = 0;
    public static int fortune = 0;
    public static float synchronization = 100; // HP

    public GJSongTrack currentTrack;
    public AudioSource audioSource;

    void Awake() {
        
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        //GJLevel now waits for another class to activate it.
    }

    public void StartLevel()
    {
        GJTrackLoader.loadTrack(TrackInfo.TrackName);
        closingTime = closingDelay;
    }

	void Update () {
        UpdateGameLogic();
        UpdateVisuals();
    }
    // ---- GAME LOGIC ----
    void UpdateGameLogic() {

        if (!isInProgress) return;

        // Use songtime instead of elapsedTime after intro offset for accuracy
        if (elapsedTime >= 0) {
            if (!audioSource.isPlaying) audioSource.Play();
            elapsedTime = audioSource.time;
        }
        else {
            elapsedTime += Time.deltaTime;
        }


        // Check if there are stil notes in the track.. if not.. do something
        if (notesSpawned >= currentTrack.noteCount) {
            closingTime -= Time.deltaTime;
            if (closingTime <= 0) {
                trackEnded = true;
            }
        }

        SpawnEnemies();

        // health cannot drop below 0 nor go beyond 100
        synchronization = Mathf.Clamp(synchronization, 0f, 100f);
        CheckWinConditions();
        CheckLoseConditions();

        accuracy = (float)hitCount / notesSpawned * 100.0f;

    }

    void CheckWinConditions() {
        if (trackEnded && isInProgress) {
            Debug.Log("Victory");
            StartCoroutine(FadeMusic(victoryEnd));
            isInProgress = false;
        }
    }

    void CheckLoseConditions() {
        if (synchronization <= 0 && isInProgress) {
            Debug.Log("Defeat");
            StartCoroutine(FadeMusic(defeatEnd));
            isInProgress = false;
        }
    }

    void UpdateVisuals() {
        HealthIndicator.reference.SetHealth(synchronization);
    }

    void SpawnEnemies() {
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
            }
            else {
                // Debug.Log("index: " + notePointers[i] + "= " + spawnerNotes[notePointers[i]]);
                float currentNote = spawnerNotes[notePointers[i]];
                // Debug.Log("currentNote: " + currentNote + " elapsedTime: " + elapsedTime);
                if (currentNote + manualOffset <= elapsedTime) {
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

    void Reset() {
        // audioSource.time = musicStartTime * audioSource.clip.length;
        // elapsedTime = musicStartTime * audioSource.clip.length;
        elapsedTime = -trackStartOffset;

        trackEnded = false;
        closingTime = closingDelay;
        notesSpawned = hitCount = 0;
        isInProgress = true;
        Time.timeScale = 1f;
        RenderSettings.ambientIntensity = 1f;
        audioSource.volume = 1f;
        audioSource.Stop();
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
    }


    // --- UI FUNCTIONS ---------

    public void ReturnToMain() {
        SceneManager.LoadScene("MainMenuScene");
    }


    public void PauseGame() {
        RenderSettings.ambientIntensity = 0f;
        Time.timeScale = 0f;
        if (audioSource.isPlaying) {
            audioSource.Pause();
        }

        isPaused = true;
        GJUIManager.instance.Show(GJUIManager.Window.LEVEL_MENU);
    }

    public void ResumeGame() {
        RenderSettings.ambientIntensity = 1f;
        Time.timeScale = 1f;
        setMonsterPause();
        audioSource.UnPause();
        isInProgress = true;
        isPaused = false;
    }

    public void ShowIntro()
    {
        GJUIManager.instance.Show(GJUIManager.Window.INTRO_MENU);
    }

    public void RestartGame() {
        GJUIManager.instance.Show(GJUIManager.Window.NONE);
        Reset();
    }

    public void victoryEnd() {
        GJUIManager.instance.Show(GJUIManager.Window.VICTORY_MENU);
    }

    public void defeatEnd() {
            GJUIManager.instance.Show(GJUIManager.Window.DEFEAT_MENU);

            if (audioSource.isPlaying) {
                audioSource.Stop();
            }

            setMonsterPause();
    }

    delegate void OutroFunction();
    IEnumerator FadeMusic(OutroFunction func) {
        while (audioSource.volume > 0) {
            yield return new WaitForSeconds(0.2f);
            audioSource.volume -= 0.2f;
        }
        func.Invoke();
    }


    // Other..

    void setMonsterPause() {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("GJMonster");
        foreach (GameObject go in monsters) {
            go.GetComponent<GJMonster>().paused = isPaused;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(Camera.main.transform.position, killRange);
    }
}
