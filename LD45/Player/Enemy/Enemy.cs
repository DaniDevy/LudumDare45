using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor {

    private Transform target;
    private Vector3 bias;
    private int c;
    private float biasStrength = 8f;
    public LayerMask whatIsWall;

    public int gunNr;

    public int desiredDist;
    public int moveAwayDist;

    protected override void Init() {
        target = PlayerMovement.Instance.transform;
        if (gunNr == -1)
            gun = GunContainer.GetRandomGun();
        else 
            gun = GunContainer.GetGun(gunNr);
        bias = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f)) * biasStrength;
        readyToFire = false;
        Invoke(nameof(FireCooldown), gun.GetFireRate() * GetFirerateMultiplier());
        health += Game.Instance.GetEnemyHealth();
        GenerateDungeon.Instance.objects.Add(gameObject);
    }

    private void Update() {
        if (target == null) return;
        Vector2 tp = target.position;
        Vector2 p = transform.position;
        float dist = Vector2.Distance(tp, p);
        if (dist > desiredDist)
            Move((target.position - transform.position) + bias);
        else if (dist < moveAwayDist) {
            Move((transform.position - target.position) + bias);
        }
        else {
            rb.velocity = Vector2.zero;
        }

        c++;
        if (c > 400) {
            bias = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f,1f)) * biasStrength;
            c = 0;
        }
        
        Aim(target.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, 20, whatIsWall);
        Debug.DrawLine(transform.position, target.position, Color.black, 1);
        if (hit.collider == null) return;
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (dist < 11f)
                Fire();
        } 
    }
}
