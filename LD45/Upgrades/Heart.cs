using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IUpgrade {
    private bool upgraded;
    public GameObject pickup;
    
    public void Upgrade() {
        if (upgraded) return;
        upgraded = true;
        PlayerMovement.Instance.Heal();
        Instantiate(pickup, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
