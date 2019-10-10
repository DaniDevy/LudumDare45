using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Helmet : MonoBehaviour, IUpgrade {
    //please dont look at my shitty redundant code LOL
    private bool upgraded;
    public GameObject pickup;
    public GameObject text;
    
    public void Upgrade() {
        if (upgraded) return;
        upgraded = true;
        PlayerMovement.Instance.helmet++;
        Instantiate(pickup, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Instantiate(text, transform.position, Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>().text = "+dash";
    }
}
