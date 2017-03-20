using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelection : MonoBehaviour {
    public GameObject prefabButton;
    public RectTransform ParentPanel;
    public GJLevel level;
    List<string> trackNames;
    // Use this for initialization
    void Start() {
        trackNames = Util.getTrackNames();

        //Create a UI Button per Track Name
        for (int i = 0; i < trackNames.Count; i++)
        {
            GameObject goButton = (GameObject)Instantiate(prefabButton);
            goButton.transform.SetParent(ParentPanel, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);

            Button tempButton = goButton.GetComponent<Button>();
            int tempInt = i;
            tempButton.GetComponentInChildren<Text>().text = trackNames[i];

            tempButton.onClick.AddListener(() => OnSongSelect(tempInt));
        }
    }

    void OnSongSelect(int i)
    {
        TrackInfo.TrackName = trackNames[i];
        level.StartLevel();
        this.gameObject.SetActive(false);
        level.RestartGame();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            OnSongSelect(5);
        }
    }
}
