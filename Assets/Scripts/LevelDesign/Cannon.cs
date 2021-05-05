using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public float rotationDamping = 6.0f;
    public float shootForce = 30f;
    public float range = 100f;
    public float shootInterval = 1.5f;
    public GameObject prefabBullet;
    private GameObject player;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        waitTime = shootInterval;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(player.transform.position.y - transform.position.y < 5f && player.transform.position.y - transform.position.y > -5f){
            Vector3 heading = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
            if(heading.sqrMagnitude < range * range){
                Quaternion rotation = Quaternion.LookRotation(heading);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
                // shoot
                waitTime -= Time.deltaTime;
                if(waitTime <= 0){
                    waitTime = shootInterval;
                    GameObject instanceBullet = Instantiate(prefabBullet, transform.position + transform.forward * 2.5f, transform.rotation);
                    instanceBullet.GetComponent<Rigidbody>().AddForce(instanceBullet.transform.forward * shootForce);
                }
            }
        }
        
        
    }
}
