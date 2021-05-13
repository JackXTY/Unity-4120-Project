using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    private Vector3 moveDirection;
    private CharacterController controller;
    private Animator animator;
    [SerializeField] public float WALK_SPEED = 0.5f;
    [SerializeField] public float RUN_SPEED = 1.0f;
    [SerializeField] public float X_ACC = 1.0f;
    [SerializeField] public float Z_ACC = 1.0f;
    public float moveSpeed;
    [SerializeField] private float speedLimit = 0.5f;
    [SerializeField] private float zSpeed = 0f;
    [SerializeField] private float xSpeed = 0f;
    private bool crouch = false;
    private bool run = false;
    private bool sprint = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
        //print("right feet: "+ animator.rightFeetBottomHeight + " left feet: " + animator.leftFeetBottomHeight);
    }

    void Move(){
        if(Input.GetButtonDown("Jump")){
            //jump = true;
            //print("pressed jump");
         //   rigidbody.AddForce(Vector3.up*JumpForce);
         //   thisAnim.SetTrigger("Jump");
        }

        //handle crouch/uncrouch
        if(Input.GetKeyDown("c")){
            //print("into crouch");
            //print("pressed crouch");
            //crouch = !crouch;
           // thisAnim.SetBool("Crouch",crouch);
        }
        
        //sprint
        if(Input.GetKey(KeyCode.LeftShift)){
            run = true;
            //crouch = false;
             speedLimit = RUN_SPEED;
        }else {
            run = false;
            speedLimit = WALK_SPEED;
        }

        //handle  WASD inputs for SMOOTH 8way animation change
        if(Input.GetKey(KeyCode.W)){
           if(!Input.GetKey(KeyCode.S)){
            zSpeed +=  Time.deltaTime * Z_ACC;
            if(zSpeed<0){
               zSpeed += 0.7f * Time.deltaTime * Z_ACC; 
            }}
        }else if(Input.GetKey(KeyCode.S)){
            zSpeed -= Time.deltaTime * Z_ACC;
            if(zSpeed>0){
               zSpeed -= 0.7f*Time.deltaTime * Z_ACC; 
            }
        }else if( Mathf.Abs(zSpeed)>=0.05){
            zSpeed += 1.7f * Time.deltaTime * Z_ACC * (zSpeed<0?1:(-1));
        }else{
            zSpeed = 0;
        }

        if(Input.GetKey(KeyCode.A)){
            if(!Input.GetKey(KeyCode.D)){
            xSpeed -= Time.deltaTime * X_ACC;
            if(xSpeed>0){
               xSpeed -= 0.7f*Time.deltaTime * X_ACC; 
            }}
        }else if(Input.GetKey(KeyCode.D)){
            xSpeed += Time.deltaTime * X_ACC;
            if(xSpeed<0){
               xSpeed += 0.7f*Time.deltaTime * X_ACC; 
            }
        }else if( Mathf.Abs(xSpeed) >= 0.03){
            xSpeed += 1.7f * Time.deltaTime * X_ACC * (xSpeed<0?1:(-1));
        }else{
            xSpeed = 0;
        }

       
        
        //handle speed limit problem,this could be done bofefore handling inputs
        if(zSpeed>speedLimit){
            zSpeed=speedLimit;
        }else if(zSpeed<-speedLimit){
            zSpeed=-speedLimit;
        }

        if(xSpeed>speedLimit){
            xSpeed=speedLimit;
        }else if(xSpeed<-speedLimit){
            xSpeed=-speedLimit;
        }

        moveDirection = new Vector3(xSpeed,0,zSpeed);
        
        controller.Move(moveDirection*Time.deltaTime*moveSpeed);

    }
}
