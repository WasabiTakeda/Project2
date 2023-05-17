using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrappedEffectController : MonoBehaviour
{
    private Animator anim;
    private void Start() {
        anim = GetComponent<Animator>();
    }
    private void Update() {
        StartCoroutine(StartEffect());
    }
    private IEnumerator StartEffect() {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("end_ice");
        Destroy(this.gameObject, 0f);
    }
}
