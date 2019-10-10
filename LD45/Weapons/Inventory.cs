using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class Inventory : MonoBehaviour {

    private Gun gun1, gun2;
    private int activeGun = 0;
    
    public void PickupGun(GameObject g) {
        if (g == null) return;
        Weapon w = (Weapon) g.GetComponent(typeof(Weapon));
        if (w.pickedUp) return;
        w.pickedUp = true;
        AudioManager.Instance.Play("PickupGun");
        g.SetActive(false);
        if (gun1 == null) {
            gun1 = ((Weapon) g.GetComponent(typeof(Weapon))).GetGun();
            PlayerStats.Instance.gun1.sprite = g.GetComponent<SpriteRenderer>().sprite;
        }
        else if (gun2 == null) {
            gun2 = ((Weapon) g.GetComponent(typeof(Weapon))).GetGun();
            PlayerStats.Instance.gun2.sprite = g.GetComponent<SpriteRenderer>().sprite;
        }
        else if (activeGun == 0) {
            gun1 = ((Weapon) g.GetComponent(typeof(Weapon))).GetGun();
            PlayerStats.Instance.gun1.sprite = g.GetComponent<SpriteRenderer>().sprite;
        }
        else {
            gun2 = ((Weapon) g.GetComponent(typeof(Weapon))).GetGun();
            PlayerStats.Instance.gun2.sprite = g.GetComponent<SpriteRenderer>().sprite;
        }

        PlayerMovement.Instance.SetGun(GetCurrentGun());
    }

    public Gun GetCurrentGun() {
        if (activeGun == 0) {
            return gun1;
        }
        return gun2;
    }

    public void SwapActive() {
        AudioManager.Instance.Play("PickupGun");
        if (activeGun == 0) {
            activeGun = 1;
            PlayerStats.Instance.gun1.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            PlayerStats.Instance.gun2.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else {
            activeGun = 0;
            PlayerStats.Instance.gun2.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.gray;
            PlayerStats.Instance.gun1.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        PlayerMovement.Instance.SetGun(GetCurrentGun());
    }

    public void ResetInventory() {
        gun1 = null;
        gun2 = null;
        activeGun = 0;
    }
}
