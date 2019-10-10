using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateDungeon : MonoBehaviour {

    public GameObject normalChest, upgradeChest, eliteChest;

    private int nRooms = 10;
    private int n = 50;
    private float scale = 30f;

    public GameObject room, enemyRoom, corridor, empty, mark, floorNr, emptyRoom;

    private Vector2 spawnPos;

    public int[,] rooms;
    public List<Tuple<Vector2, Vector2>> roomPositions;
    private List<Tuple<Vector2, Vector2>> availableRooms;

    private List<Vector2> directionsList;
    private List<Vector2> currentDirections;

    public List<GameObject> objects;
    
    public static GenerateDungeon Instance { get; set; }
    private void Awake() {
        Instance = this;
        objects = new List<GameObject>();
    }

    public void DeleteDungeon() {
        print("c: " + objects.Count);
        foreach (var g in objects) {
            Destroy(g);
        }
    }

    public void GenerateNewDungeon() {
        roomPositions = new List<Tuple<Vector2, Vector2>>();
        rooms = new int[n,n];
        
        directionsList = new List<Vector2>();
        directionsList.Add(new Vector2(0, 1));
        directionsList.Add(new Vector2(0, -1));
        directionsList.Add(new Vector2(1, 0));
        directionsList.Add(new Vector2(-1, 0));

        Vector2 pos = new Vector2(25,25);
        Vector2 size = new Vector2(Random.Range(1, 2), Random.Range(1, 2));
        Tuple<Vector2, Vector2> firstRoom = new Tuple<Vector2, Vector2>(pos, size);
        roomPositions.Add(firstRoom);
        Instantiate(floorNr, pos * scale, Quaternion.identity);
        markRoom(firstRoom, 3);
        
        for (int i = 0; i < nRooms; i++) {
            int c = 0;
            //Dont do this at home kids LOL
            while (!GenerateNewRoom()) {
                c++;
                if (c > 100) break;
            }
        }
        availableRooms = new List<Tuple<Vector2, Vector2>>(roomPositions);

        Vector2 finalRoom = availableRooms[availableRooms.Count - 1].Item1;
        rooms[(int) finalRoom.x, (int) finalRoom.y] = 3;
        FindSpawns();
        FindChests();
        ShowRooms();
    }

    private void FindChests() {
        int total = Random.Range(3, 5);
        print("total: " + total);
        
        int normalChests = Random.Range(1, 4);
        total -= normalChests;
        int upgradeChests = total;
        int eliteChests = 0;
        if (Random.Range(0f, 1f) < 0.25f) eliteChests = 1;

        for (int i = 0; i < normalChests; i++) {
            int r = Random.Range(0, availableRooms.Count);
            Vector2 pos = availableRooms[r].Item1;
            objects.Add(Instantiate(normalChest, pos * scale, Quaternion.identity));
            availableRooms.RemoveAt(r);
            rooms[(int) pos.x, (int) pos.y] = 3;
        }
        
        for (int i = 0; i < upgradeChests; i++) {
            int r = Random.Range(0, availableRooms.Count);
            Vector2 pos = availableRooms[r].Item1;
            objects.Add(Instantiate(upgradeChest, pos * scale, Quaternion.identity));
            availableRooms.RemoveAt(r);
            rooms[(int) pos.x, (int) pos.y] = 3;
        }
        for (int i = 0; i < eliteChests; i++) {
            int r = Random.Range(0, availableRooms.Count);
            Vector2 pos = availableRooms[r].Item1;
            objects.Add(Instantiate(eliteChest, pos * scale, Quaternion.identity));
            availableRooms.RemoveAt(r);
            rooms[(int) pos.x, (int) pos.y] = 3;
        }
    }

    private void ShowAvailable() {
        for (int i = 0; i < availableRooms.Count; i++) {
            print(availableRooms[i].Item1.x + ", " + availableRooms[i].Item1.y );
        }
    }

    private void FindSpawns() {
        Vector2 playerSpawn = roomPositions[0].Item1;
        spawnPos = playerSpawn;
        Vector2 bossSpawn = roomPositions[nRooms].Item1;

        availableRooms.RemoveAt(nRooms);
        availableRooms.RemoveAt(0);
        //Instantiate(mark, playerSpawn * scale, Quaternion.identity);
        objects.Add(Instantiate(mark, bossSpawn * scale, Quaternion.identity));
    }

    private void FindRandomLeaf() {
        
    }

    private bool GenerateNewRoom() {
        int roomNumber = Random.Range(0, roomPositions.Count);
        Vector2 npos = roomPositions[roomNumber].Item1;
        Vector2 nsize = roomPositions[roomNumber].Item2;
            
        int x = Random.Range(0, (int) nsize.x);
        int y = Random.Range(0, (int) nsize.y);
            
        //Find direction to create new room
        currentDirections = new List<Vector2>();
        currentDirections.Add(new Vector2(0, 1));
        currentDirections.Add(new Vector2(0, -1));
        currentDirections.Add(new Vector2(1, 0));
        currentDirections.Add(new Vector2(-1, 0));
        for (int u = 0; u < 4; u++) {
            int nr = Random.Range(0, currentDirections.Count);
            Vector2 dir = currentDirections[nr] * 2;
            currentDirections.RemoveAt(nr);
            if (!hasNeighbours(npos + dir)) {
                if (nNeighbours(npos+dir) > 2) continue;
                Vector2 p = npos + dir;
                Vector2 s = nsize;
                Tuple<Vector2, Vector2> nRoom = new Tuple<Vector2, Vector2>(p, s);
                roomPositions.Add(nRoom);
                markRoom(nRoom, 1);
                rooms[(int) (npos.x + (dir.x / 2)), (int) (npos.y + (dir.y / 2))] = 2;
                return true;
            }
        }
        return false;
    }

    private bool hasNeighbours(Vector2 pos) {
        for (int y = -1; y < 2; y++) {
            for (int x = -1; x < 2; x++) {
                Vector2 p = new Vector2(x, y) + pos;
                if (p.x < 0 || p.x > n || p.y < 0 || p.y > n) continue;
                if (rooms[(int) p.x, (int) p.y] != 0)
                    return true;
            }
        }
        return false;
    }
    
    private int nNeighbours(Vector2 pos) {
        int count = 0;

        if (rooms[(int) pos.x + 2, (int) pos.y] != 0) count++;
        if (rooms[(int) pos.x - 2, (int) pos.y] != 0) count++;
        if (rooms[(int) pos.x, (int) pos.y + 2] != 0) count++;
        if (rooms[(int) pos.x, (int) pos.y - 2] != 0) count++;

        return count;
    }

    private void markRoom(Tuple<Vector2, Vector2> newRoom, int mark) {
        Vector2 pos = newRoom.Item1;
        Vector2 size = newRoom.Item2;
        for (int x = 0; x < size.x; x++) {
            rooms[(int) pos.x + x, (int) pos.y] = mark;
        }
        for (int y = 1; y < size.y; y++) {
            rooms[(int) pos.x, (int) pos.y + y] = mark;
        }

        if (size.x == 2 && size.y == 2)
            rooms[(int) pos.x + 1, (int) pos.y + 1] = mark;
    }

    private void ShowRooms() {
        for (int x = 0; x < n; x++) {
            for (int y = 0; y < n; y++) {
                if (rooms[x, y] == 0) { //nothing
                    GameObject r = Instantiate(empty, new Vector2(x * scale, y * scale), Quaternion.identity);
                    r.transform.localScale = Vector2.one * scale;
                    objects.Add(r);
                }

                if (rooms[x, y] == 1) { //room
                    GameObject r = Instantiate(enemyRoom, new Vector2(x * scale, y * scale), Quaternion.identity);
                    r.transform.localScale = Vector2.one * scale;
                    objects.Add(r);
                }
                else if (rooms[x, y] == 2) { //corridor
                    float a = 0;
                    if (rooms[x + 1, y] == 1 || rooms[x - 1, y] == 1) a = 90;
                    if (rooms[x + 1, y] == 3 || rooms[x - 1, y] == 3) a = 90;
                    Vector2 s = corridor.transform.localScale * scale;
                    GameObject r = Instantiate(corridor, new Vector2(x * scale, y * scale), Quaternion.Euler(new Vector3(0,0,a)));
                    r.transform.localScale = s;
                    objects.Add(r);
                }
                else if (rooms[x, y] == 3) {
                    GameObject r = Instantiate(emptyRoom, new Vector2(x * scale, y * scale), Quaternion.identity);
                    r.transform.localScale = Vector2.one * scale;
                    objects.Add(r);
                }
            }
        }
    }

    public Vector2 GetSpawnPos() {
        return spawnPos * scale;
    }
}
