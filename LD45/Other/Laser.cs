using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    private Transform target;
    public LineRenderer lr;
    
    void Start() {
        target = PlayerMovement.Instance.transform;
        lr.transform.parent = null;
        lr.transform.position = Vector2.zero;
    }

    // Update is called once per frame
    void Update() {
        print(target);
        if (target == null) return;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target.position);
    }
}
