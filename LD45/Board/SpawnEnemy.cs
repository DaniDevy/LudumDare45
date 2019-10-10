using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour {

    public GameObject enemy, p, spawnPopFx;
    public ParticleSystem ps;
    
    private void Start() {
        Invoke(nameof(Spawn), Random.Range(1f, 2f));
        ParticleSystem.MainModule m = ps.main;
        m.startColor = enemy.GetComponent<SpriteRenderer>().color;
    }

    private void Spawn() {
        enemy.SetActive(true);
        Destroy(p);
        ParticleSystem ps2 = Instantiate(spawnPopFx, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule m2 = ps2.main;
        m2.startColor = enemy.GetComponent<SpriteRenderer>().color;
    }
}
