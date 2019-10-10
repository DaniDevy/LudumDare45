
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunContainer : MonoBehaviour{
    
    public static readonly Gun rustyPistol = new Gun(0.35f, 1, 0.15f, 9f, 1, "Rusty Pistol");
    public static readonly Gun shinyPistol = new Gun(0.3f, 1, 0.08f, 12f, 1, "Shiny Pistol");
    public static readonly Gun dualPistol = new Gun(0.3f, 2, 0.2f, 11f, 1, "Dual Pistol");
    public static readonly Gun revolver = new Gun(0.4f, 1, 0.05f, 14f, 2, "Revolver");
    
    public static readonly Gun smallShotgun = new Gun(0.5f, 5, 0.35f, 12f, 1, "Small Shotgun");
    public static readonly Gun shotgun = new Gun(0.45f, 6, 0.25f, 14f, 1, "Shotgun");
    public static readonly Gun sawedoffShotgun = new Gun(0.55f, 8, 0.5f, 14f, 1, "Sawed-off Shotgun");

    public static readonly Gun rifle = new Gun(0.1f, 1, 0.15f, 15f, 1, "Rifle");
    public static readonly Gun doubleRifle = new Gun(0.12f, 2, 0.45f, 15f, 1, "Double Rifle");
    public static readonly Gun coolRifle = new Gun(0.15f, 1, 0.13f, 15f, 2, "Cooler Rifle");
    public static readonly Gun sniper = new Gun(0.8f, 1, 0.0f, 25f, 10, "Sniper");

    public static List<Gun> guns;

    private void Start() {
        guns = new List<Gun>();
        guns.Add(rustyPistol);
        guns.Add(shinyPistol);
        guns.Add(dualPistol);
        guns.Add(revolver);
        guns.Add(smallShotgun);
        guns.Add(shotgun);
        guns.Add(sawedoffShotgun);
        guns.Add(rifle);
        guns.Add(doubleRifle);
        guns.Add(coolRifle);
        guns.Add(sniper);
    }

    public static Gun GetGun(int n) {
        return guns[n];
    }

    public static Gun GetRandomGun() {
        return guns[Random.Range(0, guns.Count)];
    }
}
