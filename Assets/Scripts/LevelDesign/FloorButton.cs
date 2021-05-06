using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public int index;
    public FloorButtonPuzzle puzzle;
    private void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            puzzle.touch(index);
        }
    }
}
