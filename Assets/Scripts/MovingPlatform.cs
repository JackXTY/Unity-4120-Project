using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A simple moving platform which could move in one direction
    (and then move back).
*/
public class MovingPlatform : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3 direction = new Vector3(-1f, 0f, 0f);
    public float totalDistance = 20.0f;
    public float waitTime = 2.0f;
    public bool moveAlong = false;
    private int state; // -1 => backward, -2 => backward to forward, 1 => forward, 2 => forward to backward
    private float counter;
    private bool hasPlayer = false;
    private Vector3 initialPosition;
    private Vector3 playerMovement;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        counter = totalDistance;
        direction.Normalize();
        initialPosition = transform.position;
        playerMovement = new Vector3(0f, 0f, 0f);
    }

    void FixedUpdate()
    {
        if(moveAlong && !hasPlayer){
            return;
        }
        float movement = Time.deltaTime * speed;
        float translation = 0;
        if(state == 1){
            if(counter > movement){
                translation = movement;
                counter -= movement;
            }else{
                translation = counter;
                counter = waitTime;
                state = 2;
            }
        }
        else if(state == 2){
            if(counter > Time.deltaTime){
                counter -= Time.deltaTime;
            }else{
                counter = totalDistance;
                state = -1;
            }
        }
        else if(state == -1){
            if(counter > movement){
                translation = -movement;
                counter -= movement;
            }else{
                translation = -counter;
                counter = waitTime;
                state = -2;
            }
        }else{
            if(counter > Time.deltaTime){
                counter -= Time.deltaTime;
            }else{
                counter = totalDistance;
                state = 1;
            }
        }

        if(translation != 0f){
            transform.Translate(direction * translation);
            if(moveAlong && hasPlayer){
                player.transform.Translate(direction * translation, Space.World);
                Debug.Log(direction);
                Debug.Log(translation);
            }
        }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            player = col.gameObject;
            hasPlayer = true;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            Debug.Log("Enter Player Collision");
        }
    }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            hasPlayer = false;
            // playerTransform.position = initialPosition;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            player = null;
            Debug.Log("Exit Player Collision");
        }
    }

}
