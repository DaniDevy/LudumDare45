using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Settings : MonoBehaviour {

    public TextMeshProUGUI graphicsText, musicText, sfxText;
    public PostProcessLayer ppl;
    private bool graphics, music, sfx;

    private void Start() {
        graphics = true;
    }

    public void ChangeGraphics() {
        graphics = !graphics;

        ppl.enabled = graphics;
        graphicsText.text = "Graphics - ";
        graphicsText.text += graphics ? "Fancy" : "Performance";
    }

    public void ChangeMusic() {
        bool s = AudioManager.Instance.muteMusic;
        if (s) musicText.text = "Music - off";
        else musicText.text = "Music - on";
    }

    public void ChangeSounds() {
        bool s = AudioManager.Instance.muted;
        if (s) sfxText.text = "Sounds - off";
        else sfxText.text = "Sounds - on";
    }

}
