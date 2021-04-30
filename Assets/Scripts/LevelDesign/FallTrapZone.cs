using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrapZone : MonoBehaviour
{
    public GameObject trap;

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Player"){
            trap.GetComponent<FallTrap>().fall();
            Destroy(this.gameObject);
        }
    }
}
