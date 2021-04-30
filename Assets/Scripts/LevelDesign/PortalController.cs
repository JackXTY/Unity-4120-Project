using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject GoalPortal;
    private GameObject Goal;
    void Start()
    {
        for(int i = 0; i < GoalPortal.transform.childCount; i++){
            if(GoalPortal.transform.GetChild(i).name == "goal"){
                Goal = GoalPortal.transform.GetChild(i).gameObject;
                break;
            }
        }
    }

     void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            Vector3 translation = new Vector3(Goal.transform.position.x - transform.position.x, 0f, Goal.transform.position.z - transform.position.z);
            col.gameObject.transform.Translate(translation, Space.World);
        }
    }

    // void OnCollisionExit(Collision col){
    //     if(col.gameObject.tag == "Player"){
    //         hasPlayer = false;
    //         // playerTransform.position = initialPosition;
    //         Debug.Log("Exit Player Collision");
    //     }
    // }
}
