using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.4f;
    private GameObject player = null;
    private Vector3 rotation;
    void Start()
    {
        player = null;
        rotation = new Vector3(0f, speed, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotation);
        if (player!=null){
            Vector3 oriVec = player.transform.position - transform.position;
            Vector3 translation = Quaternion.AngleAxis(speed, new Vector3(0f, 1f, 0f))*oriVec - oriVec;
            player.transform.Translate(translation, Space.World);
            player.transform.Rotate(rotation);
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Player"){
            player = other.gameObject;
        }
    }

    void OnCollisionExit(Collision other){
        if(other.gameObject.tag == "Player"){
            player = null;
        }
    }
}
