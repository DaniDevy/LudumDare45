using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    private bool done;
    private void OnTriggerEnter2D(Collider2D other) {
        if (done) return;
        if (other.gameObject.CompareTag("Player")) {
            done = true;
            Game.Instance.NextFloor();
        }
    }
}
