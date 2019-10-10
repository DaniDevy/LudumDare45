using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floor : MonoBehaviour {

    public TextMeshProUGUI text;
    public GameObject descriptionText;
    
    private void Start() {
        int floor = Game.Instance.floor;
        text.text = "Floor " + floor;
        if (floor > 1)
            descriptionText.SetActive(false);
        else descriptionText.SetActive(true);
        
        GenerateDungeon.Instance.objects.Add(gameObject);
    }
}
