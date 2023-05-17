using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treeree : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform skyPoint;
    [SerializeField] private Transform skyPoint1;
    [SerializeField] private Transform skyPoint2;
    [SerializeField] private Transform skyPoint3;
    [SerializeField] private Transform skyPoint4;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject bolt;
    private Transform player;
    private Vector3 previousPosition;
    private Vector3 lastPosition;
    private float next_time_shoot;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }
    // throw bats
    public void ThrowBat() {
        if (Time.time > next_time_shoot) {
            GameObject batProjectile = Instantiate(bat, shootPoint.transform.position, Quaternion.identity);
            batProjectile.transform.right = transform.right.normalized;
            next_time_shoot += 4f;
        }
    }  
    // strike bolt
    public void Strike() {
        Instantiate(bolt, skyPoint.position, Quaternion.identity);
        Instantiate(bolt, skyPoint1.position, Quaternion.identity);
        Instantiate(bolt, skyPoint2.position, Quaternion.identity);
        Instantiate(bolt, skyPoint3.position, Quaternion.identity);
        Instantiate(bolt, skyPoint4.position, Quaternion.identity);
    }
    private void FixedUpdate() {
        if (player == null) return;
        if (PlayerController.Instance.isMeleeAttacking && Vector2.Distance(transform.position, player.position) < 5f) {
            if (isRight()) {
                rb.velocity = new Vector2(20f, 20f);
            } else {
                rb.velocity = new Vector2(-20f, 20f);
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
}
