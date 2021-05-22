using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    void Start()
    {
        
    }
    public int index;
    public FloorButtonPuzzle puzzle;
    private void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "Player"){
            //print("touching player");
            puzzle.touch(index);
        }
    }
    
}
