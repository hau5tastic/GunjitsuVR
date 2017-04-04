using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectInEditor : MonoBehaviour {

    public bool playTrack;

    SongSelection songSelector;
    int id;

    public void Init(SongSelection _songSelector, int _id)
    {
        songSelector = _songSelector;
        id = _id;
    }
	
	// Update is called once per frame
	void Update () {
		if(playTrack)
        {
            songSelector.OnSongSelect(id);
            playTrack = false;
        }
	}
}
