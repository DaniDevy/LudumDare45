using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlighten : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        print("lighting");
    }
}
