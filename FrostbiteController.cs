using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteController : MonoBehaviour
{
    private void Start() {
        Destroy(this.gameObject, 0.8f);
    }
}
