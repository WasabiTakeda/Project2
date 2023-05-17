using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    // basic  
    [SerializeField] private Rigidbody2D rb;
    private SpriteRenderer rend;
    public float projectileSpeed = 10f;
    private GameObject player;

    public static EnemyProjectileController Instance {get; private set;}
    
    void Start()
    {
        rb.velocity = transform.right * projectileSpeed;

        if (player == null) {
            player = GameObject.FindWithTag("Player").gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
        }
    } 
}
