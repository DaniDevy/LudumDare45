using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private Gun gun;
    public int gunNr;
    public bool pickedUp;

    private void Start() {
        gun = GunContainer.GetGun(gunNr);
    }

    public Gun GetGun() {
        return gun;
    }
}
