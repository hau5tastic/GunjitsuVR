using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GJTrackLoader : MonoBehaviour {

    public static void loadTrack(string trackName) {
        readFromFile(trackName);
        loadSong();
        GJLevel.instance.currentTrack.countNotes();
        GJLevel.instance.currentTrack.sortNotes();
    }

    static void readFromFile(string trackName) {
        GJLevel.instance.currentTrack = new GJSongTrack();

        Debug.Log("Reading the track from: " + getFileName(trackName));
        if (File.Exists(getFileName(trackName))) {
            BinaryReader file =
                new BinaryReader(File.Open(getFileName(trackName), FileMode.Open));
            try {
                // Version Number
                float versionNumber = file.ReadSingle();
                if (versionNumber != Util.VERSION_NUMBER) {
                    throw new System.Exception("Editor is incompatible with this track version: " + versionNumber);
                }

                GJLevel.instance.currentTrack.songName = file.ReadString();
                TrackInfo.SongName = GJLevel.instance.currentTrack.songName;
                GJLevel.instance.currentTrack.bpm = file.ReadInt32();
                GJLevel.instance.currentTrack.startOffset = file.ReadInt32();
                GJLevel.instance.currentTrack.scrollSpeed = file.ReadInt32();


                if (GJLevel.instance.overridePlaySpeed == 0) GJLevel.instance.overridePlaySpeed = GJLevel.instance.currentTrack.scrollSpeed;

                GJLevel.instance.manualOffset = -((GJLevel.instance.spawnRange - GJLevel.instance.killRange) / (GJLevel.instance.overridePlaySpeed));
                GJLevel.instance.manualOffset += GJLevel.instance.currentTrack.startOffset / 1000f;

                GJLevel.instance.currentTrack.notes = new List<float[]>();
                GJLevel.instance.currentTrack.noteTypes = new List<int[]>();

                // For each spawner
                foreach (GJMonsterSpawner ms in GJLevel.instance.monsterSpawners) {
                    int noteCount = file.ReadInt32(); // read the number of notes of 'this' spawner
                    float[] spawnerNotes = new float[noteCount];
                    int[] spawnerNoteTypes = new int[noteCount];

                    // Load that many notes
                    for (int i = 0; i < noteCount; i++) {
                        spawnerNotes[i] = file.ReadSingle();
                        spawnerNoteTypes[i] = file.ReadInt32();
                        //Debug.Log(spawnerNoteTypes[i]);
                    }

                    GJLevel.instance.currentTrack.notes.Add(spawnerNotes);
                    GJLevel.instance.currentTrack.noteTypes.Add(spawnerNoteTypes);
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

    static void loadSong() {   
        Util.loadAudioClip(GJLevel.instance.audioSource, TrackInfo.SongName);
    }

    static string getFileName(string customTrackName) {
        return Util.TRACK_DIR_PREFIX + customTrackName;
    }
}
