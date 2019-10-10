using System;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject explosion;
    private int damage = 1;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("BlockPlayer"))
            ExplodeBullet();
        else {
            Actor a = (Actor) other.gameObject.GetComponent(typeof(Actor));
            a.Damage(-GetComponent<Rigidbody2D>().velocity, damage);
            ExplodeBullet();
        }
    }

    private void ExplodeBullet() {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetDamage(int d) {
        damage = d;
    }
}
