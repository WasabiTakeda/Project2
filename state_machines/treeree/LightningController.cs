using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource[] sounds;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // thunder
    public void Thunder() {sounds[0].Play();}
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain")
        || other.gameObject.CompareTag("Player")
        ) {
            anim.SetTrigger("hits");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this.gameObject, 1f);
        } 
    }
}
