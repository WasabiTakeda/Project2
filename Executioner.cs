using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : MonoBehaviour
{

    [SerializeField] private GameObject summons;
    
    // summons
    public void Summoning() {
        GameObject summoners = Instantiate(summons, transform.position + new Vector3(0f,5f,0f), Quaternion.identity);
        summoners.transform.right = transform.right.normalized;
    }

}
