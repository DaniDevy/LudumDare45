using UnityEngine;

public class MeleeEnemy : Actor {

    private Transform target;

    protected override void Init() {
        target = PlayerMovement.Instance.transform;
        health += Game.Instance.GetEnemyHealth();
    }

    private void Update() {
        if (target == null) return;
        Vector2 tp = target.position;
        Vector2 p = transform.position;
        float dist = Vector2.Distance(tp, p);
        Move(target.position - transform.position);
        
        Aim(target.position);
        GenerateDungeon.Instance.objects.Add(gameObject);
    }
}

