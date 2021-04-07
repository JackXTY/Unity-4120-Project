using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public float forwardRate = 3f;
    public float turnRate = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float forwardAmount = Input.GetAxis("Vertical") * forwardRate;
        transform.position += transform.forward * Time.deltaTime * forwardAmount;
        float turnAmount = Input.GetAxis("Horizontal") * turnRate;
        transform.Rotate(0, turnAmount, 0);
        if(Input.GetKeyDown("j")){
            transform.position += new Vector3(0f, 1f, 0f);
        }
        if(Input.GetKeyDown("k")){
            transform.position -= new Vector3(0f, 1f, 0f);
        }
        
    }
}
