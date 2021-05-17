using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float aliveTime = 4f;
    public int damage = 10;
    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter > aliveTime){
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            InterfaceController.Instance.Damage(10, false);
            Debug.Log("hit by bullet!");
            // col.gameObject.GetComponent<UnityChanSimpleController>().damage(damage);
        }
        Destroy(this.gameObject);
    }
}
