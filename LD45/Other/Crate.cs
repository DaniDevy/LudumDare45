using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crate : MonoBehaviour {

    public GameObject breakFx;

    public GameObject[] items;
    private Collider2D[] playerColliders;
    private Collider2D myCollider;
    public bool e;
    private bool coin;

    private void Start() {
        GenerateDungeon.Instance.objects.Add(gameObject);
    }

    public void Break() {
        Instantiate(breakFx, transform.position, Quaternion.identity);
        playerColliders = PlayerMovement.Instance.GetColliders();
        print(playerColliders);
        GameObject weapon = SelectWeapon(e);
        
        GameObject w = Instantiate(weapon, transform.position, transform.rotation);
        Vector2 dir = (transform.position - PlayerMovement.Instance.transform.position).normalized;
        w.GetComponent<Rigidbody2D>().AddForce((dir * 300) + new Vector2(Random.Range(-400, 400), Random.Range(-400, 400)));
        w.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-2000,2000));
        CameraShake.ShakeOnce(0.2f, 1.5f);
        AudioManager.Instance.Play("Hit2");
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

        if (Random.Range(0f, 1f) > 0.2f) {
            coin = true;
            return items[0];
        }
        
        int u = Mathf.FloorToInt(0 + (items.Length - 1) * Mathf.Pow(Random.Range(0f, 1f), 2));
        u += 1;

        return items[u];
    }
}
