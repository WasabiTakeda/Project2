using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    
    // get instances
    public static BossController Instance {
        get; 
        private set;
    }

    // components
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D coll;
    public AudioSource[] audioSources;

    // effects
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject bossDeathEffect;

    // references
    private GameObject player;
    [SerializeField] private LayerMask ground;
    public bool hitWall;
    private Vector3 previousPosition;
    private Vector3 lastPosition;

    // properties
    private float bossHP = 1f;
    private float currentHP;
    public float attackRange;
    public Transform attackPoint;
    public bool isInvulnerable;

    // bools
    private bool isEffectCreated = false;
    public bool isDead = false;

    // external properties
    private float meleeDamageTaken;
    private float projectileDamageTaken;
    [SerializeField] private LayerMask playerLayer;
    //[SerializeField] private GameObject bossCam;

    // draw gizmos
    void OnDrawGizmosSelected() {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void Start() {

        Instance = this;

        currentHP = bossHP;

        // get attached components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();

        BossHealthUI.Instance.BossHealthUpdate(bossHP, bossHP);

        // init components
        player = GameObject.FindWithTag("Player").gameObject;

    }

    private void FixedUpdate() {

        // update current players' damages
        meleeDamageTaken = PlayerPrefs.GetFloat(PlayerObjectsSaver.MELEE_KEY, 0.1f);
        projectileDamageTaken = PlayerPrefs.GetFloat(PlayerObjectsSaver.PROJECTILE_KEY, 0.1f);

        CheckHP();

        BossHealthUI.Instance.BossHealthUpdate(bossHP, currentHP);

        isWalled();

        // update jump & fall animation
        if (rb.velocity.y > .1f) {
            anim.SetBool("jump", true);
        } else if (rb.velocity.y < -.1f) {
            anim.SetBool("jump", false);
        } else {
            anim.SetBool("jump", false);
        }

        if (isDead) {
            StartBossFight.Instance.bossAudio.Stop();
        }

    }

    // right raycast
    // check if is collided with walls
    private void isWalled() {
        RaycastHit2D hit_left;
        RaycastHit2D hit_right;
        Vector3 startPos = coll.bounds.center + new Vector3(0f, -1f, 0f);
        // if (isRight()) {
        //     //Debug.Log("is right");
        //     hit = Physics2D.Raycast(startPos, Vector2.right, coll.bounds.extents.x + 1f, ground);
        //     Debug.DrawRay(startPos, Vector2.right * (coll.bounds.extents.x + 1f), Color.red);
        // } else {
        //     //Debug.Log("is left");
        //     hit = Physics2D.Raycast(startPos, -Vector2.right, coll.bounds.extents.x + 1f, ground);
        //     Debug.DrawRay(startPos, -Vector2.right * (coll.bounds.extents.x + 1), Color.red);
        // }
        hit_left = Physics2D.Raycast(startPos, -Vector2.right, coll.bounds.extents.x + 1f, ground);
        Debug.DrawRay(startPos, -Vector2.right * (coll.bounds.extents.x + 1), Color.green);
        hit_right = Physics2D.Raycast(startPos, Vector2.right, coll.bounds.extents.x + 1f, ground);
        Debug.DrawRay(startPos, Vector2.right * (coll.bounds.extents.x + 1), Color.blue);

        if (hit_left.collider != null) {
            if (hit_left.collider.tag == "Ground") {
                hitWall = true;
            } 
        } else if (hit_right.collider != null) {
            if (hit_right.collider.tag == "Ground") {
                hitWall = true;
            }
        }
        else {
            hitWall = false;
        }
    }
    
    // return angle on direction
    public bool isRight() {
        if (transform.position != previousPosition) {
            lastPosition = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;
        }

        bool isRight = (lastPosition.x > .1f) ? true : false;

        return isRight;
    }


    // check for current health state
    public void CheckHP() {
        if (currentHP < (bossHP / 2)) {
            GetComponent<Animator>().SetBool("isEnraged", true);
            //StartCoroutine(DeployVulnerability());
        }
    }
    private IEnumerator DeployVulnerability() {
        isInvulnerable = true;
        yield return new WaitForSeconds(1f);
        isInvulnerable = false;
    }

    // target rotation
    public void TargetedQuaternion(Vector3 target) {
        if ((target.x - transform.position.x) > 0) {
            transform.rotation = Quaternion.Euler(0,360,0);
        } else if ((target.x - transform.position.x) < 0) {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
    }

    private void TargetPlayer() {
        if (Vector2.Distance(transform.position, player.transform.position) < 10) {
            rb.position = Vector2.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), Time.deltaTime * 10f);
        }
    }

    // trigger entered
    private void OnTriggerEnter2D(Collider2D other) {
        // take player projectile damage
        if (other.gameObject.CompareTag("PlayerProjectile")) {
            BossTakeProjectile();
        } if (other.gameObject.CompareTag("Bomb")) {
            BossTakeExplosionDamage();
        }
    }

    public void BossTakeMelee() {
        hpReduction(meleeDamageTaken);

        // blood explosion
        Instantiate(blood, transform.position, Quaternion.identity);
    }

    public void BossTakeExplosionDamage() {
        hpReduction(1f);
    }

    public void BossTakeProjectile() {
        hpReduction(projectileDamageTaken);
    }

    private void hpReduction(float damage) {
        if (isInvulnerable) {
            return;
        }
        // take infliction
        currentHP -= damage;
        CameraShake.Instance.ShakeCamera(7f, 1f);
        if ((currentHP - damage) < 0f) {
            this.enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            rb.isKinematic = true;
            isDead = true;
            BossHealthUI.Instance.MoveOut();
            audioSources[3].Play();
            StartBossFight.Instance.bossCam.SetActive(false);
            StartBossFight.Instance.bossAudio.Stop();
            anim.SetBool("isDead", true);
            if (isEffectCreated == false) {
                Instantiate(bossDeathEffect, transform.position, Quaternion.identity);
                isEffectCreated = true;
            }
            Destroy(this.gameObject, 4f);
        }
    }

    // audios
    public void PlayMeleeAudio1() {
        audioSources[0].Play();
    }
    public void PlayMeleeAudio2() {
        audioSources[1].Play();
    }
    public void PlayAbilityAudio1() {
        audioSources[2].Play();
    }
    public void PlayAbilityAudio2() {
        audioSources[5].Play();
    }
    public void Phase2() {
        audioSources[4].Play();
    }
    public void JumpSound() {
        audioSources[6].Play();
    }
    public void StepSound() {
        audioSources[7].Play();
    }

    // inflictions
    // damage player with melee
    public void meleeDamagePlayer() {
        // check for player overlapping
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayer) {
            player.GetComponent<PlayerController>().playerTakeMeleeDamage(0.1f);
        }
    }

}
