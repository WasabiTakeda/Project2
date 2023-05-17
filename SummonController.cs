using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonController : MonoBehaviour
{
    
    // components
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D coll;

    // properties
    public float summonHP = 0.5f;
    public float currentSummonHP;

    // external properties
    private GameObject player;
    private float meleeDamageTaken;
    private float projectileDamageTaken;

    private void Start() {

        // get attached components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();

        player = GameObject.FindWithTag("Player").gameObject;

    }

    private void FixedUpdate() {

        // update current players' damages
        meleeDamageTaken = PlayerPrefs.GetFloat(PlayerObjectsSaver.MELEE_KEY, 0.1f);
        projectileDamageTaken = PlayerPrefs.GetFloat(PlayerObjectsSaver.PROJECTILE_KEY, 0.1f);

        TargetPlayer();

    }

    private void TargetPlayer() {
        if (player != null) {
            if (Vector2.Distance(transform.position, player.transform.position) < 20) {
                rb.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), Time.deltaTime * 5f);
            }
        }
    }

    // trigger entered
    private void OnTriggerEnter2D(Collider2D other) {
        // take player projectile damage
        if (other.gameObject.CompareTag("PlayerProjectile") || other.gameObject.CompareTag("Bomb")) {
            SummonTakeProjectile();
        }
    }
    // collision entered
    private void OnCollisionEnter2D(Collision2D collision) {
        // take player projectile damage
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().playerTakeProjectileDamage(0.2f);
            Destroy(this.gameObject);
        }
    }

    public void SummonTakeMelee() {
        hpReduction(meleeDamageTaken);
    }

    public void SummonTakeProjectile() {
        hpReduction(projectileDamageTaken);
    }

    private void hpReduction(float damage) {
        // take infliction
        currentSummonHP -= damage;
        if ((currentSummonHP - damage) < 0f) {
            Destroy(this.gameObject);
        }
    }

}
