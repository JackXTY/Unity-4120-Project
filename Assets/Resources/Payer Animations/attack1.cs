using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack1 : StateMachineBehaviour
{
    ThridPersonController player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("Vanguard").GetComponent<ThridPersonController>();
        player.attackLock(new Vector3(0,0,0), 1);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.attackUnlock();
    }

}
