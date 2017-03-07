using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSelectTutorial : GJSelection {

    public string levelName;
    AsyncOperation async;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) OnSelect();
    }

    public override void OnSelect() {
        StartLoading();
    }


    public void StartLoading() {
        StartCoroutine("load");
    }

    IEnumerator load() {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        async = SceneManager.LoadSceneAsync(levelName);
        async.allowSceneActivation = true;
        Debug.Log(async.progress+"%");

        yield return async;
    }

    public void ActivateScene() {
        async.allowSceneActivation = true;
    }
}
