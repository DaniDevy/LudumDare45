using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    
    public TextMeshProUGUI gold;
    public Transform hearts;

    public SpriteRenderer gun1, gun2;

    public static PlayerStats Instance { get; set; }
    private void Awake() {
        Instance = this;
    }

    private void Update() {
        gold.text = "" + PlayerMovement.Instance.GetGold();
        UpdateHearts();
    }
    
    private void UpdateHearts() {
        int h = PlayerMovement.Instance.GetHealth();

        for (int i = 0; i < 5; i++) {
            if (i < h)
                hearts.GetChild(i).GetChild(0).gameObject.SetActive(true);
            else hearts.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
}
