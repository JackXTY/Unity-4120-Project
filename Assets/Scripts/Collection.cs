using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    public string name = "art_1";
    public int number = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            // do something here
            // maybe add a new weapon, or try to implement a collection system
            Debug.Log("Get the collection " + name);
            Destroy(this.gameObject);
        }
    }
}
