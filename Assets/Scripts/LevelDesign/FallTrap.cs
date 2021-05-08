using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrap : MonoBehaviour
{
    public int damageValue = 10;
    private Rigidbody rigid;
    private SphereCollider collider;
    private GameObject zone;
    private GameObject player;
    private float Timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
        zone = transform.GetChild(0).gameObject;
    }

    public void fall(){
        rigid.useGravity = true;
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag == "Player"){
            player = other.gameObject;
            player.GetComponent<UnityChanSimpleController>().damage(damageValue);
            StartCoroutine(goingToDestroy());
        }
    }

    IEnumerator goingToDestroy(){
        yield return new WaitForSeconds(Timer);
        Destroy(this.gameObject);
    }
}
