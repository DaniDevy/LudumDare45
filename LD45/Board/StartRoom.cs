using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartRoom : MonoBehaviour {

    public SpriteRenderer doorSprite;
    public GameObject colliders, roomfade, enemy;
    private bool started;

    private int waves, currentWave;
    private float roomSize;

    private List<GameObject> enemies;

    private bool readyToCheck = true;

    private void Awake() {
        enemies = new List<GameObject>();
        roomSize = 28f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (started) return;
        if (other.gameObject.CompareTag("Player")) {
            CameraMovement.Instance.SetRoom(transform);
            CameraShake.ShakeOnce(0.5f, 2f);
            AudioManager.Instance.Play("DoorOpen");
            doorSprite.sortingOrder = 1;
            colliders.SetActive(true);
            roomfade.SetActive(true);
            started = true;
            waves = Game.Instance.GetWaveAmount();
            currentWave = 1;
        }
    }

    private void FixedUpdate() {
        if (!started || !readyToCheck) return;
        
        CheckEnemyList();
        if (enemies.Count == 0) {
            if (currentWave > waves) {
                FinishRoom();
                return;
            }
            SpawnEnemies();
            currentWave++;
            readyToCheck = false;
        }
    }

    private void CheckEnemyList() {
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i] == null)
                enemies.RemoveAt(i);
        }
    }

    private void SpawnEnemies() {
        print("spawning bois");
        int n = Game.Instance.GetEnemyAmount();
        for (int i = 0; i < n; i++) {
            Invoke(nameof(SpawnOneEnemy), Random.Range(0, 7));
        }

        Invoke(nameof(GetCheckReady), 7f);
    }

    private void GetCheckReady() {
        readyToCheck = true;
    }

    private void SpawnOneEnemy() {
        GameObject e = SelectEnemy();
        Vector3 p = (transform.position + new Vector3(Random.Range(-roomSize / 2, roomSize / 2),Random.Range(-roomSize / 2, roomSize / 2), 0));
        enemies.Add(Instantiate(e, p, Quaternion.identity).transform.GetChild(0).gameObject);
    }

    private GameObject SelectEnemy() {
        return Game.Instance.GetEnemy();
    }

    public void FinishRoom() {
        doorSprite.sortingOrder = -1;
        colliders.SetActive(false);
        roomfade.SetActive(false);
        CameraMovement.Instance.SetRoom(null);
        AudioManager.Instance.Play("DoorClose");
        Destroy(this);
    }
}
