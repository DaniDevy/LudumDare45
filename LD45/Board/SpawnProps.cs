using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProps : MonoBehaviour {

    public GameObject barrel;
    public GameObject crate;
    public GameObject wall;
    public GameObject lava;

    public bool walls;
    
    void Start() {
        
        
        int crates = Random.Range(0, 3);

        for (int i = 0; i < crates; i++) {
            Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
            GenerateDungeon.Instance.objects.Add(Instantiate(crate, transform.position + pos, Quaternion.identity));
        }

        if (walls) {
            int walls = Random.Range(0, 4); 
            for (int i = 0; i < walls; i++) {
                Vector3 pos = new Vector3(Random.Range(-7, 7), Random.Range(-7, 7));
                Vector3 r = new Vector3(0, 0, Random.Range(1, 3) * 90);
                GenerateDungeon.Instance.objects.Add(Instantiate(wall, transform.position + pos, Quaternion.Euler(r)));
            }
        }
        
    }
    
}
