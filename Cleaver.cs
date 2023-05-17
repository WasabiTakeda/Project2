using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaver : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject firebreath;
    private float next_time_shoot;
    private float next_time_breathe;
    private Transform player;
    private Rigidbody2D rb;
    private Vector3 previousPosition;
    private Vector3 lastPosition;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }
    private void FixedUpdate() {
        if (player == null) return;
        if (PlayerController.Instance.isMeleeAttacking && Vector2.Distance(transform.position, player.position) < 5f) {
            if (isRight()) {
                rb.velocity = new Vector2(10f, 10f);
            } else {
                rb.velocity = new Vector2(-10f, 10f);
            }
        }
    }
    // return angle on direction
    private bool isRight() {
        if (transform.position != previousPosition) {
            lastPosition = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;
        }

        bool isRight = (lastPosition.x > .1f) ? true : false;

        return isRight;
    }
    // throw fireball
    public void ThrowFireball() {
        if (Time.time > next_time_shoot) {
            // shoot ...
            GameObject enemyProjectile = Instantiate(fireball, shootPoint.transform.position, Quaternion.identity);
            enemyProjectile.transform.right = transform.right.normalized;
            next_time_shoot += 2f;
        }
    }
    // fire breathing
    public void BreatheFire() {
        if (Time.time > next_time_breathe) {
            // breathe ...
            GameObject fireBreath = Instantiate(firebreath, shootPoint.transform.position, Quaternion.identity);
            fireBreath.transform.right = transform.right.normalized;
            next_time_breathe += 2f;
        }
    }
}
