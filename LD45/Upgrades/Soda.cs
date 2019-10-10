using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Soda : MonoBehaviour, IUpgrade {
    
    private bool upgraded;
    public GameObject pickup;
    public GameObject text;
    
    public void Upgrade() {
        if (upgraded) return;
        upgraded = true;
        PlayerMovement.Instance.soda++;
        Instantiate(pickup, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Instantiate(text, transform.position, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>().text = "+fire rate";
    }
}
