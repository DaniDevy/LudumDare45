using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {
    
    public GameObject player;

    public GameObject menuUI, gameUI, deadUI;

    public GameObject[] enemies;

    public int floor = 1;

    public bool playing;
    public static Game Instance { get; set; }

    public TextMeshProUGUI doneText;

    private void Awake() {
        Instance = this;
    }

    public void NextFloor() {
        GenerateDungeon.Instance.DeleteDungeon();
        GenerateDungeon.Instance.GenerateNewDungeon();
        PlayerMovement.Instance.gameObject.transform.position = GenerateDungeon.Instance.GetSpawnPos();
        CameraMovement.Instance.StartRound();
        floor++;
        Minimap.Instance.LoadMap();
    }

    public void StartGame() {
        GenerateDungeon.Instance.GenerateNewDungeon();
        Vector2 spawnPos = GenerateDungeon.Instance.GetSpawnPos();
        GameObject p = Instantiate(player, spawnPos, Quaternion.identity);
        CameraMovement.Instance.SetPlayer(p.transform);
        CameraMovement.Instance.StartRound();
        Timer.Instance.StartTimer();
        playing = true;
        floor = 1;
        PlayerMovement.Instance.ResetPlayer();
        Minimap.Instance.LoadMap();
    }

    public float GetFirerateMultiplier() {
        float f = Timer.Instance.GetMinutes() / 3;
        if (f == 0) f = 1;
        
        if (f == 5) f = 5;
        return 5 / f;
    }

    public float GetSpeedMultiplier() {
        float f = Timer.Instance.GetMinutes() / 30;
        if (f > 0.5f) f = 0.5f;
        
        return 0.5f + f;
    }

    public int GetEnemyAmount() {
        int min = Timer.Instance.GetMinutes();
        int mi = 1 + (int) Mathf.Floor(min / 2);
        int ma = (int) Mathf.Floor(min / 1.5f);
        if (ma < 3) ma = 3;
        if (ma > 10) ma = 10;
        if (mi < 2) mi = 2;
        print("min: " + mi + ", m: " + ma);
        return Random.Range(mi, ma);
    }

    public int GetWaveAmount() {
        int min = Timer.Instance.GetMinutes();
        int ma = (int) Mathf.Floor(min / 4);
        if (ma > 4) ma = 4;
        if (ma < 2) ma = 2;
        return Random.Range(1, ma);
    }

    public GameObject GetEnemy() {
        int min = Timer.Instance.GetMinutes() / 4;
        int m = 2 + min;
        if (m > enemies.Length) m = enemies.Length;
        print("max: " + m);
        int max = Random.Range(0, m);
        print("r: " + max);

        return enemies[max];
    }

    public void PlayerDied() {
        deadUI.SetActive(true);
        gameUI.SetActive(false);
        GenerateDungeon.Instance.DeleteDungeon();
        doneText.text = "You made it to floor " + floor;
        PlayerStats.Instance.gun1.sprite = null;
        PlayerStats.Instance.gun2.sprite = null;
    }

    public int GetEnemyHealth() {
        int min = Timer.Instance.GetMinutes() / 3;
        return min;
    }
    
    public void ExitGame() {
        Application.Quit(1);
    }
}
