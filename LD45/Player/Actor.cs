using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

public class Actor : MonoBehaviour {

    public float speed;
    protected Rigidbody2D rb;
    public GameObject killFx, bullet, coin;
    
    protected Inventory inventory;

    protected int dashCooldown;

    protected bool dashing;
    protected Vector2 dashDir;
    protected float dashSpeed = 15f;
    protected int dashDuration = 35;
    protected int dashC;

    protected Gun gun = GunContainer.rustyPistol;
    protected bool readyToFire = true;

    protected SpriteRenderer sr;
    protected Color dColor;

    [SerializeField] 
    protected int health = 5;

    public ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    protected virtual void Init() { }
    private void Awake() {
        ps = GetComponentInChildren<ParticleSystem>();
        em = ps.emission;
        rb = GetComponent<Rigidbody2D>();
        Init();
        sr = GetComponent<SpriteRenderer>();
        dColor = sr.color;
    }

    protected void Aim(Vector3 target) {
        Vector2 dir = transform.position - target;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0,0, angle + 90));
    }

    protected void Move(Vector2 dir) {
        float pen = 1;
        if ((dir.x > 0.5f || dir.x < -0.5f) && (dir.y > 0.5f || dir.y < -0.5f)) {
            pen = 1.35f;
        }

        rb.velocity = dir.normalized * GetSpeed() * pen;
    }

    protected virtual float GetSpeed() {
        return speed;
    }

    protected void Dash(Vector2 dir) {
        if (dashCooldown > 0) return;
        AudioManager.Instance.Play("Dash");
        float pen = 1;
        if ((dir.x > 0.5f || dir.x < -0.5f) && (dir.y > 0.5f || dir.y < -0.5f)) {
            pen = 0.8f;
        }
        rb.angularVelocity = 0f;
        em.enabled = true;
        dashing = true;
        dashDir = dir * pen;
        dashC = 0;
        dashCooldown = 200;
        CameraShake.ShakeOnce(0.25f, 1);
    }

    protected void StopDash() {
        dashDir = Vector2.zero;
        dashing = false;
        em.enabled = false;
        rb.angularVelocity = 0f;
    }

    protected virtual void KillActor() {
        GameObject fx = Instantiate(killFx, transform.position, Quaternion.identity);
        ParticleSystem.MainModule mm = fx.GetComponent<ParticleSystem>().main;
        mm.startColor = dColor;
        
        AudioManager.Instance.Play("Explosion1");

        int a = Random.Range(1, 7);
        for (int i = 0; i < a; i++) {
            GameObject c = Instantiate(coin, transform.position, Quaternion.identity);
            GenerateDungeon.Instance.objects.Add(c);
            c.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-500, 500), Random.Range(-500, 500)));
        }
        
        Destroy(gameObject);
    }

    public virtual void Damage(Vector2 dir, int damage) {
        health -= damage;
        if (health < 1) 
            KillActor();

        AudioManager.Instance.Play("EnemyHit1");
        
        CancelInvoke(nameof(ResetColor));
        sr.color = Color.white;
        Invoke(nameof(ResetColor), 0.05f);
    }

    private void ResetColor() {
        sr.color = dColor;
    }

    protected void Fire() {
        if (gun == null) return;
        if (!readyToFire) return;
        readyToFire = false;
        AudioManager.Instance.Play("Shoot");
        for (int i = 0; i < gun.GetBullets(); i++) {
            GameObject b = Instantiate(bullet, transform.position + (transform.up / 1.5f), transform.rotation);
            if (gameObject.layer == LayerMask.NameToLayer("Player")) {
                b.layer = LayerMask.NameToLayer("PlayerBullet");
            } else b.layer = LayerMask.NameToLayer("EnemyBullet");
            Rigidbody2D brb = b.GetComponent<Rigidbody2D>();

            b.GetComponent<SpriteRenderer>().color = dColor;
            
            ((Bullet) b.GetComponent(typeof(Bullet))).SetDamage(gun.GetDamage());

            float s = gun.GetDamage() / 2;
            if (s > 0.5f) s = 0.5f;
            b.transform.localScale *= 1 + s;
            
            GenerateDungeon.Instance.objects.Add(b);

            Vector2 dir = transform.rotation * Vector2.up;
            Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-gun.GetSpread(), gun.GetSpread());
            brb.velocity = (dir + pdir) * gun.GetSpeed() * GetSpeedMultiplier();
        }

        float f = gun.GetFireRate();
        Invoke(nameof(FireCooldown), gun.GetFireRate() * GetFirerateMultiplier());
    }

    protected void FireCooldown() {
        readyToFire = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (dashing) {
            if (other.gameObject.layer == LayerMask.NameToLayer("BlockPlayer")) {
                if (!other.gameObject.CompareTag("Crate"))
                    StopDash();
            }
        }
    }

    protected float GetSpeedMultiplier() {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            return Game.Instance.GetSpeedMultiplier();
        return 1;
    }
    
    protected float GetFirerateMultiplier() {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            return Random.Range(2, Game.Instance.GetFirerateMultiplier());
        return GetFireRate();
    }

    protected virtual float GetFireRate() {
        return 2;
    }
}
