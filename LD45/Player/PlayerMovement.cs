using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class PlayerMovement : Actor {

    public GameObject goldFx;
    public static PlayerMovement Instance { get; set; }
    private bool invincible;
    private int cooldown;
    private int pCooldown;

    public int soda, helmet, sneaker;

    public GameObject currentWeapon;
    public GameObject chest;
    
    private int dashDamage = 1;

    private int gold;
    
    private Vector2 pushDir;
    protected override void Init() {
        inventory = new Inventory();
        Instance = this;
        gun = inventory.GetCurrentGun();
    }

    private void Update() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Aim(mousePos);

        if (pCooldown > 0)
            return;
        if (dashing)
            return;
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Move(new Vector2(x, y));

        //Shooting
        if (Input.GetMouseButton(0))
            Fire();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Dash(((Vector3) mousePos - transform.position).normalized);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            inventory.SwapActive();
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (chest != null) {
                Chest c = (Chest) chest.GetComponent(typeof(Chest));
                if (gold >= c.price) {
                    c.Open();
                    gold -= c.price;
                }
                else {
                    CameraShake.ShakeOnce(0.15f,1.5f);
                    AudioManager.Instance.Play("Error");
                }
            }
            else if (currentWeapon != null) {
                inventory.PickupGun(currentWeapon);
                currentWeapon = null;
            }
        }
    }

    private void FixedUpdate() {
        if (dashCooldown > 0) dashCooldown--;

        if (invincible) {
            float a = Mathf.PingPong(Time.time * 10, 0.6f) + 0.3f;
            if (a > 0.7f)
                sr.color = dColor;
            else sr.color = Color.black;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a / 1.3f);
            
            cooldown--;
            pCooldown--;
            
            if (cooldown < 1) {
                invincible = false;
            }

            if (pCooldown > 0) {
                rb.velocity = pushDir;
                return;
            }

            if (cooldown == 0) sr.color = dColor;
        }
        if (dashing) {
            rb.velocity = (dashDir * dashSpeed) * (1f + (helmet / 4f));
            dashC++;
            float r = transform.localRotation.eulerAngles.z;
            ParticleSystem.MainModule m = ps.main;
            m.startRotationZ = 0.01745f * -r;
            if (dashC > dashDuration) {
                StopDash();
            }
        }
    }

    public Rigidbody2D GetRb() {
        return rb;
    }

    public override void Damage(Vector2 dir, int damage) {
        if (invincible || dashing) return;
        base.Damage(dir, 1);
        invincible = true;
        cooldown = 80;
        pCooldown = 25;
        pushDir = -dir;
        StopDash();
        AudioManager.Instance.Play("PlayerHit");
    }

    protected override void KillActor() {
        base.KillActor();
        Destroy(gameObject);
        Game.Instance.playing = false;
        Game.Instance.PlayerDied();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
            if (other.gameObject.CompareTag("Coin")) {
                Destroy(other.gameObject);
                AudioManager.Instance.Play("Coin");
                Instantiate(goldFx, transform.position, Quaternion.identity);
                gold++;
            }
            
            //Weapons
            if (other.gameObject.CompareTag("Gun")) {
                currentWeapon = other.gameObject;
            }
            else if (other.gameObject.CompareTag("Chest"))
                chest = other.gameObject;
            else if (other.gameObject.CompareTag("Upgrade")) {
                ((IUpgrade) other.GetComponent(typeof(IUpgrade))).Upgrade();
                AudioManager.Instance.Play("Pickup");                
            }
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            if (dashing) {
                Actor e = (Actor) other.GetComponent(typeof(Actor));
                e.Damage(rb.velocity, dashDamage + helmet);
                CameraShake.ShakeOnce(0.2f, 2.5f);
            }
            else {
                Damage(rb.velocity, dashDamage);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("BlockPlayer")) {
            if (other.gameObject.CompareTag("Crate")) {
                if (dashing)
                    ((Crate) other.gameObject.GetComponent(typeof(Crate))).Break();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
            if (other.gameObject.CompareTag("Gun")) {
                currentWeapon = null;
            }
            else if (other.gameObject.CompareTag("Chest"))
                chest = null;
        }
    }

    public void Heal() {
        if (health == 5) {
            AudioManager.Instance.Play("Coin");
            gold += 10;
            return;
        }
        health++;
    }

    public void SetGun(Gun g) {
        gun = g;
    }

    public int GetGold() {
        return gold;
    }

    public int GetHealth() {
        return health;
    }

    public void ResetPlayer() {
        inventory.ResetInventory();
        soda = 0;
        sneaker = 0;
        helmet = 0;
        gun = null;
        health = 5;
        gold = 20;
        inventory = new Inventory();
    }

    protected override float GetFireRate() {
        int s = soda + 1;
        float r = 1.4f / s;

        if (r < 0.25f) r = 0.3f;
        return r;
    }
    protected override float GetSpeed() {
        float s = speed + (sneaker / 2);
        if (s > 20) s = 20;
        return s;
    }

    public Collider2D[] GetColliders() {
        return GetComponentsInChildren<Collider2D>();
    }
}
