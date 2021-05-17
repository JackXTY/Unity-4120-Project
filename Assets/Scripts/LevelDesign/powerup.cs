using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class powerup : MonoBehaviour
{
	public bool HP_up = false;
	public Text myText;
	private float timeToAppear = 1f;
	private float timeWhenDisappear;

	public void EnableText()
	{
    	myText.enabled = true;
    	timeWhenDisappear = Time.time + timeToAppear;
	}

    // Start is called before the first frame update
    void Start()
    {
        myText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player;
        Vector3 heading;
        player = GameObject.FindGameObjectWithTag("Player");
        heading = player.transform.position - transform.position;
        if (heading.sqrMagnitude < 0.6)
        {
            HP_up = true;
        }
        if (HP_up == true)
    	{
    		if (!myText.enabled)
    			EnableText();
    		if (myText.enabled && (Time.time > timeWhenDisappear))
		    {
		        myText.enabled = false;
		    }
    	}
    }
}
