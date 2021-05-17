using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int attack1 = 10;
    public int attack2 = 20;
    private int attackWay = 0;
    // true - attack1 (small attack)
    // false - attack2 (large attack)

    public void changeAttack(int way){
        attackWay = way;
        //Debug.Log("change " + way);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag=="Monster" && attackWay!=0){
            int attackValue = attack1;
            if(attackWay == 2){
                attackValue = attack2;
            }
            other.gameObject.GetComponent<MonsterManager>().hit(attackValue);
        }
    }
}
