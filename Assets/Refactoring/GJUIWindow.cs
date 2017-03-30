﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJUIWindow : MonoBehaviour  {

    public GJUIManager.Window ID;

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = ViveControllerInput.Instance.ControllerCamera;
    }
}
