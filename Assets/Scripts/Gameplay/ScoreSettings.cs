using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GJScore
{
    public enum GJAccuracy
    {
        PERFECT,
        GREAT,
        GOOD,
        OK
    }

    public class ScoreSettings : MonoBehaviour
    {
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
    }
}