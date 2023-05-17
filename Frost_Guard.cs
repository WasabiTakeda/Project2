using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost_Guard : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private Vector3 previousPosition;
    private Vector3 lastPosition;
    [SerializeField] private GameObject iceshard_1;
    [SerializeField] private GameObject iceshard_2;
    [SerializeField] private Transform shootPoint_1;
    [SerializeField] private Transform shootPoint_2;
    [SerializeField] private GameObject frostBite;
    [SerializeField] private AudioSource evade;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }
    // evade
    public void Dash() {
        evade.Play();
        rb.velocity = transform.right * 20f;
    }
    // throw ice
    public void ThrowIceShards() {
        GameObject ice_1 = Instantiate(iceshard_1, shootPoint_1.position, Quaternion.identity);
        ice_1.transform.right = transform.right.normalized;
        GameObject ice_2 = Instantiate(iceshard_2, shootPoint_2.position, Quaternion.identity);
        ice_2.transform.right = transform.right.normalized;
    }
    // bite frost
    public void BiteFrost() {
        GameObject frostsmoke = Instantiate(frostBite, transform.position+new Vector3(0f,1f), Quaternion.identity);
        frostsmoke.transform.right = transform.right.normalized;
    }
    private void Update() {
        if (player == null) return;
        if (PlayerController.Instance.isMeleeAttacking && Vector2.Distance(transform.position, player.position) < 8f) {
            //rb.velocity = transform.right * -20f;
            rb.velocity = new Vector2(rb.transform.right.x * -20f, 20f);
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
