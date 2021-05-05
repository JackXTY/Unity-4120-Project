using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorControl : MonoBehaviour
{
	private Animator thisAnim;
	public float scanRange = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    	GameObject player;
        Vector3 heading;
        player = GameObject.FindGameObjectWithTag("Player");
        heading = player.transform.position - transform.position;
        //Debug.Log(heading.sqrMagnitude);
        //Debug.Log(player);
        if (heading.sqrMagnitude < scanRange * scanRange)
        {
        	Debug.Log("Near");
        	thisAnim.SetBool ("character_nearby", true); 
        }
        else
        {
        	Debug.Log("Far");
        	thisAnim.SetBool ("character_nearby", false); 
        }
    }
}
