using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameEnd : MonoBehaviour
{
	private Rigidbody rigid;
    private BoxCollider collider;
	public bool end = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }

    void Update () {
    	if (end == true)
        	Application.Quit();
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            end = true;
        }
    }
}
