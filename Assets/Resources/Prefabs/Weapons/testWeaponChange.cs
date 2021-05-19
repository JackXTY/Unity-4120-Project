using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testWeaponChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag=="Player"){
            Debug.Log("SET WEAPON!!!!");
            this.gameObject.GetComponent<Weapon>().setWeapon();
        }
    }
}
