using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {

    public TextMeshProUGUI text, status;
    private float timer = 0;
    
    public static Timer Instance { get; set; }

    private void Awake() {
        Instance = this;
    }

    public void StartTimer() {
        timer = 0;
    }

    void Update() {
        if (!Game.Instance.playing) return;
        timer += Time.deltaTime;
 
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");
     
        text.text = string.Format("{0}:{1}", minutes, seconds);

        status.text = StatusText(Mathf.Floor(timer / 60));
    }

    private string StatusText(float f) {
        if (f < 2) return "very easy";
        if (f < 4) return "easy";
        if (f < 8) return "medium";
        if (f < 12) return "hard";
        if (f < 16) return "very hard";
        if (f < 20) return "impossible";
        if (f < 25) return "oh shit";
        if (f < 30) return "very oh shit";
        return "f";
    }

    public int GetMinutes() {
        return (int) Mathf.Floor(timer / 60);
    }
    
}
