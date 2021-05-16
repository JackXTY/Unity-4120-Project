using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    private Vector3 moveDirection;
    private CharacterController controller;
    private Animator animator;

    [SerializeField] public float WALK_SPEED = 0.7f;
    [SerializeField] public float CROUCH_SPEED = 0.4f;
    [SerializeField] public float RUN_SPEED = 1.1f;
    [SerializeField] public float X_ACC = 1.0f;
    [SerializeField] public float Z_ACC = 1.0f;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float speedLimit = 1.0f;
    [SerializeField] private float speedMultiplyer;
    [SerializeField] private float zSpeed = 0f;
    [SerializeField] private float xSpeed = 0f;
    private Vector2 turn;

    [SerializeField] public float gravity;
    [SerializeField] public float groundDistance;
    [SerializeField] public float groundOffset;
    [SerializeField] public LayerMask groundMask;
    private Vector3 velocity;
    [SerializeField] public float jumpVelocity;

    [SerializeField] private bool isGrounded;
    private int fall = 0;
    [SerializeField] private bool previouslyGrounded = true;
    private float previousY = 0.0f;
    private bool crouch = false;
    private bool run = false;
    private bool sprint = false;
    private bool jump = false;

    private bool ascending = false;

    private bool attack1 = false;
    private bool attack2 = false;


    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    void FixedUpdate()
    {
        Move();
        //print("right feet: "+ animator.rightFeetBottomHeight + " left feet: " + animator.leftFeetBottomHeight);
    }
    
    void Move(){
        handlePlayerRotation();

        isGrounded = Physics.CheckSphere(transform.position + controller.center - new Vector3(0,controller.height / 2 - groundOffset,0) ,  groundDistance, groundMask);


        if(isGrounded && velocity.y < 0 ){
            //print("Grounded?");
            velocity.y = -0.01f;
        }else if(controller.collisionFlags == CollisionFlags.Above && ascending){
            print("head bonked?");
            velocity.y = -0.1f;
            ascending = false;
        }else if(!isGrounded && controller.velocity.y<0){
            ascending = false;
        }else if(!isGrounded && controller.velocity.y>0){
            ascending = true;
        }

        

        if(fall == 0 && !isGrounded){
            print("start falling check");
            fall = 1;
        }
        
        if(!isGrounded && fall>=1 && velocity.y - previousY <= gravity * Time.deltaTime * 0.95f && velocity.y - previousY >= gravity * Time.deltaTime * 1.1f){
            fall += 1;
            print("fall frame count: " + fall);
        }else{
            fall = -1;
        }

        if(previouslyGrounded != isGrounded && previouslyGrounded == true){
            fall = 0;
        }
        
        previouslyGrounded = isGrounded;
        previousY = velocity.y;

        if(isGrounded){
            if(Input.GetButtonDown("Jump")){
                jump = true;
                print("pressed jump");
                velocity.y += jumpVelocity;
                animator.SetTrigger("Jump");
                isGrounded = false;
            }
        }
        

        if(isGrounded){
            //print("on ground");
            if(Input.GetKey(KeyCode.LeftShift)){
                run = true;
                crouch = false;
                //crouch = false;
                speedMultiplyer = RUN_SPEED;
            }else if(Input.GetKeyDown("c"))
            {
                crouch = !crouch;
                speedMultiplyer = CROUCH_SPEED;
            }else {
                run = false;
                speedMultiplyer = WALK_SPEED;
            }  

            //handle  WASD inputs for SMOOTH 8way movement
            if(Input.GetKey(KeyCode.W)){
            if(!Input.GetKey(KeyCode.S)){
                zSpeed +=  Time.deltaTime * Z_ACC * speedMultiplyer;
                if(zSpeed<0){
                zSpeed += 0.5f * Time.deltaTime * Z_ACC * speedMultiplyer; 
                }}
            }else if(Input.GetKey(KeyCode.S)){
                zSpeed -= Time.deltaTime * Z_ACC * speedMultiplyer;
                if(zSpeed>0){
                zSpeed -= 0.5f*Time.deltaTime * Z_ACC * speedMultiplyer; 
                }
            }else if( Mathf.Abs(zSpeed)>=0.05){
                zSpeed += 1.5f * Time.deltaTime * Z_ACC * (zSpeed<0?1:(-1)) * speedMultiplyer;
            }else{
                zSpeed = 0;
            }

            if(Input.GetKey(KeyCode.A)){
                if(!Input.GetKey(KeyCode.D)){
                xSpeed -= Time.deltaTime * X_ACC * speedMultiplyer;
                if(xSpeed>0){
                xSpeed -= 0.5f*Time.deltaTime * X_ACC* speedMultiplyer; 
                }}
            }else if(Input.GetKey(KeyCode.D)){
                xSpeed += Time.deltaTime * X_ACC * speedMultiplyer;
                if(xSpeed<0){
                xSpeed += 0.5f*Time.deltaTime * X_ACC * speedMultiplyer; 
                }
            }else if( Mathf.Abs(xSpeed) >= 0.03){
                xSpeed += 1.5f * Time.deltaTime * X_ACC * (xSpeed<0?1:(-1))* speedMultiplyer;
            }else{
                xSpeed = 0;
            }

        
            
            //handle speed limit problem,this could be done bofefore handling inputs
            if(zSpeed>speedLimit * speedMultiplyer){
                zSpeed=speedLimit * speedMultiplyer;
            }else if(zSpeed<-speedLimit * speedMultiplyer){
                zSpeed=-speedLimit * speedMultiplyer;
            }

            if(xSpeed>speedLimit * speedMultiplyer){
                xSpeed=speedLimit * speedMultiplyer;
            }else if(xSpeed<-speedLimit * speedMultiplyer){
                xSpeed=-speedLimit * speedMultiplyer;
            }

            moveDirection = new Vector3(xSpeed,0,zSpeed);
            moveDirection = transform.TransformDirection(moveDirection);

            if(Input.GetKeyDown(KeyCode.J)){
                attack1 = true;
                weapon.changeAttack(1);
                Debug.Log("Attack 1!!");
            }
            else if(Input.GetKeyDown(KeyCode.K)){
                attack2 = true;
                weapon.changeAttack(2);
                Debug.Log("Attack 2!!");
            }
            //else if(!(animator.GetCurrentAnimatorStateInfo(0).IsName("attack1")||
            //     animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))){
            //     weapon.changeAttack(0);
            // }
        }
        
        velocity.y += gravity * Time.deltaTime;

        //print("after move velocity y is " + velocity.y);

        controller.Move(moveDirection*Time.deltaTime*moveSpeed);

        controller.Move( velocity * Time.deltaTime);



        updateAnimator();
    }

    void handlePlayerRotation(){
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        transform.rotation = Quaternion.Euler(0f,turn.x,0);
    }

    void updateAnimator(){
        if(fall > 15){
            print("falling");
            animator.SetTrigger("Fall");
            fall = -1;
            jump = false;
        }
        if(jump){
            animator.SetTrigger("Jump");
            jump = false;
            previouslyGrounded = false;
        }
        
        animator.SetFloat("zSpeed", zSpeed / (speedLimit*speedMultiplyer));
        animator.SetFloat("xSpeed", xSpeed / (speedLimit*speedMultiplyer));
        animator.SetBool("Crouch",crouch);
        animator.SetBool("Sprint",run);
        animator.SetBool("Grounded",isGrounded);
        if(attack1){
            animator.SetTrigger("Attack1");
            attack1 = false;
        }
        if(attack2){
            animator.SetTrigger("Attack2");
            attack2 = false;
        }
    }
}
