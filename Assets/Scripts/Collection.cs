using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection : MonoBehaviour
{
    //public string name = "art_1";
    //public int number = 1;

    public Item item;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            GameManager.Instance.PickUpItem(item);
            Destroy(this.gameObject);
        }
    }
}
