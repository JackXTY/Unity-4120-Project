using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator thisAnim;
    private Rigidbody rigid;
    public float JumpForce = 500;
    public float groundDistance = 0.3f;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        // thisAnim.SetTrigger("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        thisAnim.SetFloat("Speed", v);
        thisAnim.SetFloat("TurningSpeed", h * 3.0f);

        if(Input.GetButtonDown("Jump")) {
            rigid.AddForce(Vector3.up * JumpForce);
            thisAnim.SetTrigger("Jump");
        }
        if(Physics.Raycast(transform.position+(Vector3.up*0.1f), Vector3.down, groundDistance, whatIsGround)){
            thisAnim.SetBool("grounded", true);
            thisAnim.applyRootMotion = true;
        } else {
            thisAnim.SetBool("grounded", false);
        }
    }
}
