using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        Invoke(nameof(KillSelf), 1f);
    }

    private void KillSelf() {
        Destroy(gameObject);
    }
}

   
