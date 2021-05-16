using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameEnd : MonoBehaviour
{
	private Rigidbody rigid;
	public bool end = false;
	public Text myText;
	private float timeToAppear = 4f;
	private float timeWhenDisappear;


	public void EnableText()
	{
    	myText.enabled = true;
    	timeWhenDisappear = Time.time + timeToAppear;
	}


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        myText.enabled = false;
    }

    void Update () {
    	if (end == true)
    	{
    		if (!myText.enabled)
    			EnableText();
    		if (myText.enabled && (Time.time > timeWhenDisappear))
		    {
		        myText.enabled = false;
		        Application.Quit();
		        Debug.Log("quit game");
		    }
    	}
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            end = true;
        }
    }
}
