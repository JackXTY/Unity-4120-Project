using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject GoalPortal;
    private GameObject Goal;
    public bool sameLevel = true;
    void Start()
    {
        for(int i = 0; i < GoalPortal.transform.childCount; i++){
            if(GoalPortal.transform.GetChild(i).name == "goal"){
                Goal = GoalPortal.transform.GetChild(i).gameObject;
                break;
            }
        }
        if(Goal == null){
            Debug.Log("Can find relative goal for " + GoalPortal.name);
        }
    }

     private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            Debug.Log(this.gameObject.name+" -> "+GoalPortal.name);
            col.gameObject.GetComponent<CharacterController>().enabled = false;
            if(sameLevel){
                Vector3 translation = new Vector3(Goal.transform.position.x - col.gameObject.transform.position.x, 0f, Goal.transform.position.z - col.gameObject.transform.position.z - 1f);
                col.gameObject.transform.Translate(translation, Space.World);
            }else{
                Vector3 translation = Goal.transform.position - col.gameObject.transform.position;
                col.gameObject.transform.Translate(translation, Space.World);
            }
            
            col.gameObject.GetComponent<CharacterController>().enabled = true;
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
