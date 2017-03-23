using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJUIManager : MonoBehaviour {

    public static GJUIManager instance = null;

    public enum Window { NONE, INTRO_MENU, LEVEL_MENU, VICTORY_MENU, DEFEAT_MENU }

    [Header("Window Elements")]
    [SerializeField]
    GJUIWindow[] uiWindows;


    void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void Show (Window windowID) {
        if (windowID == Window.NONE) {
            foreach (GJUIWindow window in uiWindows) {
                window.gameObject.SetActive(false);
                RenderSettings.ambientIntensity = 1f;
            }
        } else {

            bool exists = false;
            foreach (GJUIWindow window in uiWindows) {
                if (window.ID == windowID) {
                    window.gameObject.SetActive(true);
                    RenderSettings.ambientIntensity = 0f;
                    exists = true;
                }
                else {
                    window.gameObject.SetActive(false);
                }
            }

            if (!exists) RenderSettings.ambientIntensity = 1f;

        }

    }
   
}
