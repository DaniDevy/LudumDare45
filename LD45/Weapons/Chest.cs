using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour {

    public GameObject[] weapons;
    private Collider2D[] playerColliders;
    private Collider2D myCollider;
    public bool e;
    public int price;
    
    public void Open() {
        playerColliders = PlayerMovement.Instance.GetColliders();
        print(playerColliders);
        GameObject weapon = SelectWeapon(e);
        GameObject w = Instantiate(weapon, transform.position, transform.rotation);
        Vector2 dir = (transform.position - PlayerMovement.Instance.transform.position).normalized;
        w.GetComponent<Rigidbody2D>().AddForce((dir * 3000) + new Vector2(Random.Range(-4000, 4000), Random.Range(-4000, 4000)));
        w.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-2000,2000));
        CameraShake.ShakeOnce(0.3f, 2f);
        AudioManager.Instance.Play("Pickup");
        myCollider = w.GetComponent<Collider2D>();
        RemoveCollision();
        gameObject.SetActive(false);
        GenerateDungeon.Instance.objects.Add(w);
    }

    private void RemoveCollision() {
        for (int i = 0; i < playerColliders.Length; i++) {
            Physics2D.IgnoreCollision(playerColliders[i], myCollider, true);
        }
        Invoke(nameof(ResumeCollision), 0.8f);
    }

    private void ResumeCollision() {
        print("start colliding again");
        for (int i = 0; i < playerColliders.Length; i++) {
            Physics2D.IgnoreCollision(playerColliders[i], myCollider, false);
        }
    }

    private GameObject SelectWeapon(bool e) {
        if (e) {
            return weapons[Random.Range(0, weapons.Length)];
        }
        
        int u = Mathf.FloorToInt(0 + (weapons.Length) * Mathf.Pow(Random.Range(0f, 1f), 2));
        int sum = 0;

        return weapons[u];
    }

}
