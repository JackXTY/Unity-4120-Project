using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//jump functions currently only work for forward jump are not yet implemented

public class    TestingAnimationController : MonoBehaviour
{

    //vatiables related to camera and character y rotation
    private Vector2 turn;

    //variables related to animator controller
    private bool addForce = false;
    private float preY=1;
    public float acc;
    public float angularAcc;
    private float speedLimit = 0.5f;
    private float h=0;
    private float v=0;
    private bool crouch=false;
    private bool isCrouching=false;
    private bool grounded=true;
    private bool jump = false;
    private Animator thisAnim;
    public float JumpForce = 500;
    public float groundDistance = 0.1f;
    public LayerMask whatIsGround;
    private CapsuleCollider capsule;
    private float capsuleHeight;
    private Vector3 capsuleCenter;
    private bool pGround = true;
    private bool sprint = false;
    private bool attack1 = false;
    private bool attack2 = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        thisAnim = GetComponent<Animator>();
        //need to handle the collider to handle crouching animation
        capsule = GetComponent<CapsuleCollider>();
        capsuleHeight = capsule.height;
        capsuleCenter = capsule.center;
        print("capsule.height is "+capsule.height);
        print("capsule.center is "+capsule.center.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        handlePlayerRotation();
        handleInput();
        detectGround();
        handleCrouchCapsuleCollider();
        updateAnimator();
        //print("the gound situation is curenlty " + grounded);
    }


    void handlePlayerRotation(){
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(0f,turn.x,0);
    }
    void detectGround(){
        if(Physics.Raycast(capsule.transform.TransformPoint(capsule.center),Vector3.down,capsule.height/2 + groundDistance,whatIsGround)){
            if(!pGround){
                print("landing");
            }
            pGround = grounded = true;
            //print("normal grounded");
            thisAnim.applyRootMotion = true;
            preY = 1.0f;
        }else if(Mathf.Abs(GetComponent<Rigidbody>().velocity.y)<=0.05){
            if(Mathf.Abs(GetComponent<Rigidbody>().velocity.y-preY)<=0.05){
                print((GetComponent<Rigidbody>().velocity.y));
                print(preY);
                pGround = grounded = true;
                thisAnim.applyRootMotion = true;
                preY = 1.0f;
                print("stuck grounded" + grounded);
                
            }else{
                preY = GetComponent<Rigidbody>().velocity.y;
            } 
        }
        else{
            //print("ungrounded");
            grounded = false;
            thisAnim.applyRootMotion = false;
            //thisAnim.SetBool("Grounded",false);
        }
    }


    void handleInput(){
        //inputs for jump function are not yet implemented
        
        if(Input.GetButtonDown("Jump")){
            jump = true;
            //print("pressed jump");
         //   rigidbody.AddForce(Vector3.up*JumpForce);
         //   thisAnim.SetTrigger("Jump");
        }

        //handle crouch/uncrouch
        if(Input.GetKeyDown("c")){
            //print("into crouch");
            print("pressed crouch");
            crouch = !crouch;
           // thisAnim.SetBool("Crouch",crouch);
        }
        
        //sprint
        if(Input.GetKey(KeyCode.LeftShift)){
            sprint = true;
            crouch = false;
        }else {
            sprint = false;
        }

        //handle  WASD inputs for SMOOTH 8way animation change
        if(Input.GetKey(KeyCode.W)){
           if(!Input.GetKey(KeyCode.S)){
            v +=  Time.deltaTime * acc;
            if(v<0){
               v += 0.7f * Time.deltaTime * acc; 
            }}
        }else if(Input.GetKey(KeyCode.S)){
            v -= Time.deltaTime * acc;
            if(v>0){
               h -= 0.7f*Time.deltaTime * acc; 
            }
        }else if( Mathf.Abs(v)>=0.05){
            v += 1.7f * Time.deltaTime * acc * (v<0?1:(-1));
        }else{
            v = 0;
        }

        if(Input.GetKey(KeyCode.A)){
            if(!Input.GetKey(KeyCode.D)){
            h -= Time.deltaTime * angularAcc;
            if(h>0){
               h -= 0.7f*Time.deltaTime * angularAcc; 
            }}
        }else if(Input.GetKey(KeyCode.D)){
            h += Time.deltaTime * angularAcc;
            if(h<0){
               h += 0.7f*Time.deltaTime * angularAcc; 
            }
        }else if( Mathf.Abs(h) >= 0.03){
            h += 1.7f * Time.deltaTime * angularAcc * (h<0?1:(-1));
        }else{
            h = 0;
        }

       
        
        //handle speed limit problem,this could be done bofefore handling inputs
        if(v>speedLimit){
            v=speedLimit;
        }else if(v<-speedLimit){
            v=-speedLimit;
        }

        if(h>speedLimit){
            h=speedLimit;
        }else if(h<-speedLimit){
            h=-speedLimit;
        }

        //if(thisAnim.GetCurrentAnimatorStateInfo(1).IsName("Idle"))
        if(Input.GetKeyDown(KeyCode.J)){
            attack1 = true;
            Debug.Log("Attack 1!!");
        }
        else if(Input.GetKeyDown(KeyCode.K)){
            attack2 = true;
            Debug.Log("Attack 2!!");
        }
        
        

        
        //thisAnim.SetFloat("Speed", v);
        //thisAnim.SetFloat("Turning Speed", h);

    }

// update Capsule Collider for Crouch animation
    void handleCrouchCapsuleCollider(){
        if(crouch && grounded){
            if(isCrouching) return;
            isCrouching = true;
            capsule.height = capsule.height / 1.4f;
            capsule.center = capsule.center / 1.55f;
           // print("adjusted collider");
        }else if(grounded){
            capsule.height = capsuleHeight;
            capsule.center = capsuleCenter;
            isCrouching = false;
            crouch = false;
        }
    } 

    void handleInAirCapsuleCollider(){
        capsule.height = capsule.height / 1.1f;
        capsule.center = capsule.center / 0.6f;
    }
    void updateAnimator(){
        thisAnim.SetFloat("zSpeed", v);
        thisAnim.SetFloat("xSpeed", h);
        thisAnim.SetBool("Crouch",isCrouching);
        thisAnim.SetBool("Sprint",sprint);
        

        //functions related to jumping are not yet implemented
        
        thisAnim.SetBool("Grounded",grounded);
        

        if(grounded && jump){
            print("jump");
            thisAnim.SetTrigger("Jump");
            //handleInAirCapsuleCollider();
            GetComponent<Rigidbody>().AddForce(Vector3.up*JumpForce);
            grounded = false;
            jump = false;
            pGround = false;
            print("addforce");
        }
        //not working properly due to collider issues
        else if(!grounded && pGround){
            print("fall off");
            //thisAnim.SetTrigger("Fall");
            //handleInAirCapsuleCollider();
            pGround = false;
        }

        
        if(attack1){
            thisAnim.SetTrigger("Attack1");
            attack1 = false;
        }
        if(attack2){
            thisAnim.SetTrigger("Attack2");
            attack2 = false;
        }

        
        // if(thisAnim.GetCurrentAnimatorStateInfo(1).IsName("null")){
        //     Debug.Log("null state");
        // }else{
        //     Debug.Log("attack state");
        // }


    }
/*    public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (grounded && Time.deltaTime > 0)
			{
				Vector3 v = (thisAnim.deltaPosition) / Time.deltaTime;
                Quaternion r = thisAnim.rootRotation;
				// we preserve the existing y part of the current velocity.
				v.y = GetComponent<Rigidbody>().velocity.y;
				GetComponent<Rigidbody>().velocity = v;
                transform.rotation = r;
		}
        }*/

}
