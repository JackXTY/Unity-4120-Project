using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int hp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void hit(int hit){
        Debug.Log("monster get hit " + hit);
        if(hit >= hp){
            DestroyMonster();
        }else{
            hp -= hit;
        }
    }

    private void DestroyMonster(){
        // maybe some animation there, but I'm afraid that we won't do so.
        Destroy(this.gameObject);
    }
}
