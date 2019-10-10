using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sneakers : MonoBehaviour, IUpgrade {

    private bool upgraded;
    public GameObject pickup;

    public GameObject text;
    
    public void Upgrade() {
        if (upgraded) return;
        upgraded = true;
        PlayerMovement.Instance.sneaker++;
        Instantiate(pickup, transform.position, Quaternion.identity);
        Destroy(gameObject);

        Instantiate(text, transform.position, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>().text = "+speed";
    }
}
