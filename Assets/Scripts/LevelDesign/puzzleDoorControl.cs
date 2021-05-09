using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleDoorControl : MonoBehaviour
{
	private Animator thisAnim;
	public GameObject puzzle;
    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        puzzle = GameObject.FindGameObjectWithTag("Puzzle");
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzle.GetComponent<FloorButtonPuzzle>().win == true)
        {
             thisAnim.SetBool ("character_nearby", true); 
        }
    }
}
