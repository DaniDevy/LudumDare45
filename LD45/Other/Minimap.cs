using System;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    private List<Tuple<Vector2, Vector2>> layout;

    public Material mat;
    
    private GameObject player;

    public GameObject room;
    public Transform p;
    private LineRenderer lr;
    private Transform playerT;

    public static Minimap Instance { get; set; }
    private void Awake() {
        Instance = this;
        /*
        lr = p.gameObject.AddComponent<LineRenderer>();
        lr.startColor = Color.white;
        lr.startWidth = 10f;
        lr.transform.parent = p;
        lr.sortingOrder = 10;
        lr.sortingLayerName = "UI";
        lr.material = mat;
        */
    }

    public void LoadMap() {
        for (int i = p.childCount - 1; i >= 0; i--) {
            Destroy(p.GetChild(i).gameObject);
        }
        layout = GenerateDungeon.Instance.roomPositions;
        Vector2 pos2 = PlayerMovement.Instance.transform.position / 30f;
        player = Instantiate(room, pos2, Quaternion.identity);
        player.transform.parent = p;
        player.transform.localPosition = pos2;
        player.transform.localScale *= 0.25f;
        player.GetComponent<SpriteRenderer>().color = Color.blue;
        player.GetComponent<SpriteRenderer>().sortingOrder = 5;

        for (int i = 0; i < layout.Count; i++) {
            Vector2 pos = layout[i].Item1 - new Vector2(25, 25);
            pos *= 8;
            GameObject g = Instantiate(room, pos, Quaternion.identity);
            g.transform.parent = p;
            g.transform.localPosition = pos;
            g.transform.localScale *= 0.25f;
        }

        playerT = PlayerMovement.Instance.transform;
    }

    private void Update() {
        if (player == null || playerT == null) return;
        Vector3 pos = ((playerT.position / 30f) - new Vector3(25, 25)) * 8f;
        Vector3 pPos = new Vector3((int) pos.x, (int)pos.y);
        player.transform.localPosition = pPos;

        /*
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, player.transform.position);
*/
        for (int i = 0; i < layout.Count; i++) {
            
        }
    }
}
