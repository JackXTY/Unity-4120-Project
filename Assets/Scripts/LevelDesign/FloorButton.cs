using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    public int index;
    public FloorButtonPuzzle puzzle;
    private bool touched;

    void Update()
    {
    	GameObject player;
        Vector3 heading;
        player = GameObject.FindGameObjectWithTag("Player");
        heading = player.transform.position - transform.position;
        if (heading.sqrMagnitude < 0.2 && !touched)
        {
        	puzzle.touch(index);
        	touched = true;
        }
        else if (heading.sqrMagnitude > 0.2)
        {
        	touched = false;
        }
    }

}
