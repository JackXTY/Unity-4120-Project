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
        GameObject player;
        Vector3 heading;
        player = GameObject.FindGameObjectWithTag("Player");
        heading = player.transform.position - transform.position;
        if (heading.sqrMagnitude < 0.4)
        {
            end = true;
            InterfaceController.Instance.end_menu.GetComponent<EndMenu>().EndGame();

        }
    	//if (end == true)
    	//{
    	//	if (!myText.enabled)
    	//		EnableText();
    	//	if (myText.enabled && (Time.time > timeWhenDisappear))
		   // {
		   //     myText.enabled = false;
		   //     Application.Quit();
		   //     Debug.Log("quit game");
		   // }
    	//}
    }

}
