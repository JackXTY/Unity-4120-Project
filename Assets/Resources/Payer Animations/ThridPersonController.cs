using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridPersonController : MonoBehaviour
{
    public static ThridPersonController Instance;
    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private Vector3 localDirection;
    private CharacterController controller;
    private Animator animator;

    [SerializeField] public float WALK_SPEED = 0.7f;
    [SerializeField] public float CROUCH_SPEED = 0.4f;
    [SerializeField] public float RUN_SPEED = 1.1f;
    [SerializeField] public float AIR_SPEED = 0.2f;
    [SerializeField] public float X_ACC = 1.0f;
    [SerializeField] public float Z_ACC = 1.0f;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float speedLimit = 0.7f;
    [SerializeField] private float speedMultiplyer = 0.7f;
    [SerializeField] private float zSpeed = 0f;
    [SerializeField] private float xSpeed = 0f;
    private Vector2 turn;
    private Vector3 jumpDirection;
    private bool isMoving = false;

    [SerializeField] public float gravity;
    [SerializeField] public float groundDistance;
    [SerializeField] public float groundOffset;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] private Vector3 velocity;
    [SerializeField] public float jumpVelocity;

    [SerializeField] private bool isGrounded;
    private float fall = -1f;
    [SerializeField] private bool previouslyGrounded = true;
    private float previousY = 0.0f;
    private bool crouch = false;
    private bool run = false;
    private bool sprint = false;
    private bool jump = false;
    private bool airJump = false;

    private bool ascending = false;

    private bool attack1 = false;
    private bool attack2 = false;
    private bool inAttack = false;

    bool stop;

    //private InterfaceController ui;

    public Weapon weapon;

    private float maxStamina;
    private float stamina;
    [SerializeField] public float runCost;
    [SerializeField] public float sprintCost;
    [SerializeField] public float sprintTimeLimit;
    [SerializeField] public float jumpCost;
    [SerializeField] public float airJumpCost;
    [SerializeField] public float attack1Cost;
    [SerializeField] public float attack2Cost;
    [SerializeField] public float staminaRecoveryRate;

    private GameObject shoulder; 
    private float lookUpLimit = 40.0f;
    private float lookDownLimit = -30.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        stop = false;
        if (Instance == null)
        {
            Instance = this;
        }
        turn.x = transform.rotation.eulerAngles.y;
        turn.y = 0f;
        maxStamina = (int)InterfaceController.Instance.max_stamina;
        stamina = (int)InterfaceController.Instance.stamina;
        // ui = GameObject.Find("InterfaceCanvas").GetComponent<InterfaceController>();
        // stamina = 100f; 
        // ui.SetStamina(1f);
        shoulder = transform.GetChild(3).gameObject;
        if (InterfaceController.Instance.start_menu.active)   //at the start of the game
        {
            ResumeMouseControl();    //open mouse control
        }
    }

    public void ResumeMouseControl()
    {
        stop = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("can use mouse!");
    }

    public void DisableMouseControl()
    {
        stop = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame

    void Update()
    {
        if (!stop)
        {
            Move();

            updateAnimator();
        }
    }



    void FixedUpdate()
    {
        if (!stop)
        {

            transform.rotation = Quaternion.Euler(0f, turn.x, 0);
            shoulder.transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0f);
            controller.Move(moveDirection * Time.deltaTime * moveSpeed + velocity * Time.deltaTime);
            //Debug.Log("stamina: "+stamina.ToString());

        }
    }

    public void SetStamina(float value)
    {
        stamina = value;
    }

    void Move()
    {


        isGrounded = (Physics.CheckSphere(transform.position + controller.center - new Vector3(0, controller.height / 2 - groundOffset, 0), groundDistance, groundMask)) || controller.isGrounded;

        //preserve momentem
        if(!inAttack){
            localDirection = transform.InverseTransformDirection(moveDirection);
            zSpeed = localDirection.z;
            xSpeed = localDirection.x;
        }
        

        handlePlayerRotation();

        //handle headbonk and on ground y velocity
        if (isGrounded && velocity.y < 0)
        {
            //print("Grounded?");
            velocity.y = -0.05f;
        }
        else if (controller.collisionFlags == CollisionFlags.Above && ascending)
        {
            print("head bonked?");
            velocity.y = -0.1f;
            ascending = false;
        }
        else if (!isGrounded && controller.velocity.y < 0)
        {
            ascending = false;
        }
        else if (!isGrounded && controller.velocity.y > 0)
        {
            ascending = true;
        }


        //start falling check


        if (!isGrounded && fall >= 0f && velocity.y <= gravity * 0.15f)
        {
            if ((!controller.isGrounded))
            {
                fall += Time.deltaTime;
                print("fall frame count: " + fall);
            }
        }

        if (previouslyGrounded != isGrounded && previouslyGrounded == true && velocity.y <= 0)
        {
            fall = 0f;
        }
        else if (velocity.y > 0)
        {
            fall = -1f;
        }

        //possibility of falling


        //record current gound status and y celocity
        previouslyGrounded = isGrounded;
        previousY = velocity.y;



        //jump priority is the highest among all groudn movements
        //in air speed change ability is reduced
        //speed limit is also instanly changed to run speed for better in air movement
        //TODO: disable jump rotation
        if (isGrounded)
        {
            airJump = false;

            if (speedMultiplyer == AIR_SPEED)
            {
                if (run)
                {
                    speedLimit = RUN_SPEED;
                    speedMultiplyer = RUN_SPEED;
                }
                else
                {
                    speedMultiplyer = WALK_SPEED;
                    speedLimit = WALK_SPEED;
                }
            }

            if (Input.GetButtonDown("Jump") && stamina >= jumpCost * 0.9)
            {
                jumpDirection = transform.forward;
                stamina -= jumpCost;
                jump = true;
                print("pressed jump");
                velocity.y = jumpVelocity;
                if (Input.GetKey(KeyCode.W))
                {
                    zSpeed += Z_ACC * WALK_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    zSpeed -= Z_ACC * WALK_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    xSpeed -= Z_ACC * WALK_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    xSpeed += Z_ACC * WALK_SPEED * 0.2f;
                }
                //animator.SetTrigger("Jump");
                isGrounded = false;
                previouslyGrounded = false;
                speedMultiplyer = AIR_SPEED;
                speedLimit = RUN_SPEED;
                crouch = false;
            }

        }
        else if (!isGrounded && !airJump && stamina >= airJumpCost * 0.9)
        {

            if (Input.GetButtonDown("Jump"))
            {
                stamina -= airJumpCost;
                jump = true;
                airJump = true;
                print("pressed air jump");
                velocity.y = jumpVelocity;
                if (Input.GetKey(KeyCode.W))
                {
                    zSpeed += X_ACC * RUN_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    zSpeed -= X_ACC * RUN_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    xSpeed -= X_ACC * RUN_SPEED * 0.2f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    xSpeed += X_ACC * RUN_SPEED * 0.2f;
                }
                //animator.SetTrigger("Jump");

            }
        }


        if (isGrounded && !inAttack)
        {
            //print("on ground");
            if (Input.GetKeyDown(KeyCode.LeftShift) && stamina >= 0)
            {
                run = !run;
                crouch = false;
                if (run)
                {
                    speedMultiplyer = RUN_SPEED;
                    speedLimit = RUN_SPEED;
                }
                else
                {
                    speedMultiplyer = WALK_SPEED;
                    speedLimit = WALK_SPEED;
                }
            }
            else if (Input.GetKeyDown("c"))
            {
                crouch = !crouch;
                run = false;
                if (crouch)
                {
                    speedMultiplyer = CROUCH_SPEED;
                    speedLimit = CROUCH_SPEED;
                }
                else
                {
                    speedMultiplyer = WALK_SPEED;
                    speedLimit = WALK_SPEED;
                }

            }else if(!run && !crouch){
                speedMultiplyer = WALK_SPEED;
                speedLimit = WALK_SPEED;
            }

            // attack can only be performed on ground
            if (Input.GetKeyDown(KeyCode.J) && stamina >= attack1Cost * 0.8f)
            {
                stamina -= attack1Cost;
                attack1 = true;
                weapon.changeAttack(1);
                Debug.Log("Attack 1!!");
                crouch = false;
            }
            else if (Input.GetKeyDown(KeyCode.K) && stamina >= attack2Cost * 0.8f)
            {
                stamina -= attack2Cost;
                attack2 = true;
                weapon.changeAttack(2);
                Debug.Log("Attack 2!!");
                crouch = false;
            }
            //else if(!(animator.GetCurrentAnimatorStateInfo(0).IsName("attack1")||
            //     animator.GetCurrentAnimatorStateInfo(0).IsName("attack2"))){
            //     weapon.changeAttack(0);
            // }
        }

        if (stamina <= 100 && stamina >= 0)
        {
            if (run && isGrounded)
            {
                Debug.Log("stamina decrease");
                stamina -= runCost * Time.deltaTime;
            }
            else if (isGrounded && !inAttack)
            {
                //todo: stamina recover delay
                stamina += staminaRecoveryRate * Time.deltaTime;
            }
        }

        if (stamina > 100)
        {
            stamina = 100;
        }
        else if (stamina < 0)
        {
            stamina = 0;
            run = false;
        }

        InterfaceController.Instance.SetStamina(stamina / maxStamina);

        if (!inAttack)
        {
            if (Input.GetKey(KeyCode.W))
            {
                isMoving = true;
                if (!Input.GetKey(KeyCode.S))
                {
                    zSpeed += Time.deltaTime * Z_ACC * speedMultiplyer;
                    if (zSpeed < 0 && isGrounded)
                    {
                        zSpeed += 0.5f * Time.deltaTime * Z_ACC * speedMultiplyer;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                isMoving = true;
                zSpeed -= Time.deltaTime * Z_ACC * speedMultiplyer;
                if (zSpeed > 0 && isGrounded)
                {
                    zSpeed -= 0.5f * Time.deltaTime * Z_ACC * speedMultiplyer;
                }
            }
            else if (Mathf.Abs(zSpeed) >= 0.05 * speedMultiplyer)
            {
                zSpeed += 0.5f * Time.deltaTime * Z_ACC * (zSpeed < 0 ? 1 : (-1)) * speedMultiplyer;
                if (isGrounded)
                {
                    zSpeed += 0.8f * Time.deltaTime * Z_ACC * (zSpeed < 0 ? 1 : (-1)) * speedMultiplyer;
                }
            }
            else
            {
                zSpeed = 0;
            }

            if (Input.GetKey(KeyCode.A))
            {
                isMoving = true;
                if (!Input.GetKey(KeyCode.D))
                {
                    xSpeed -= Time.deltaTime * X_ACC * speedMultiplyer;
                    if (xSpeed > 0 && isGrounded)
                    {
                        xSpeed -= 0.5f * Time.deltaTime * X_ACC * speedMultiplyer;
                    }
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                isMoving = true;
                xSpeed += Time.deltaTime * X_ACC * speedMultiplyer;
                if (xSpeed < 0 && isGrounded)
                {
                    xSpeed += 0.5f * Time.deltaTime * X_ACC * speedMultiplyer;
                }
            }
            else if (Mathf.Abs(xSpeed) >= 0.05 * speedMultiplyer)
            {
                xSpeed += 0.5f * Time.deltaTime * X_ACC * (xSpeed < 0 ? 1 : (-1)) * speedMultiplyer;
                if (isGrounded)
                {
                    xSpeed += 0.8f * Time.deltaTime * X_ACC * (xSpeed < 0 ? 1 : (-1)) * speedMultiplyer;
                }
            }
            else
            {
                xSpeed = 0;
            }

            if (!isMoving && run)
            {
                run = false;
            }

            isMoving = false;
            if (zSpeed > speedLimit)
            {
                zSpeed = speedLimit;
            }
            else if (zSpeed < -speedLimit)
            {
                zSpeed = -speedLimit;
            }

            if (xSpeed > speedLimit)
            {
                xSpeed = speedLimit;
            }
            else if (xSpeed < -speedLimit)
            {
                xSpeed = -speedLimit;
            }
            moveDirection = new Vector3(xSpeed, 0, zSpeed);
            moveDirection = transform.TransformDirection(moveDirection);
        }
        

        velocity.y += gravity * Time.deltaTime;

        
        //stores the global direction of the move to preserve momentem


    }

    void handlePlayerRotation()
    {
        turn.x += Input.GetAxis("Mouse X");
        turn.y += Input.GetAxis("Mouse Y");
        if(turn.y>lookUpLimit){
            turn.y = lookUpLimit;
        }else if(turn.y<lookDownLimit){
            turn.y = lookDownLimit;
        }
        //TODO: look up and look down

    }

    void updateAnimator()
    {
        if (fall > 0.2f)
        {
            print("falling");
            animator.SetTrigger("Fall");
            fall = -1f;
            jump = false;
            speedMultiplyer = AIR_SPEED;
            speedLimit = RUN_SPEED;
            crouch = false;
        }
        if (jump)
        {
            animator.SetTrigger("Jump");
            jump = false;
            previouslyGrounded = false;
        }

        animator.SetFloat("zSpeed", zSpeed / (speedLimit));
        animator.SetFloat("xSpeed", xSpeed / (speedLimit));
        animator.SetBool("Crouch", crouch);
        animator.SetBool("Sprint", run);
        animator.SetBool("Grounded", isGrounded);
        if (attack1)
        {
            animator.SetTrigger("Attack1");
            attack1 = false;
        }
        if (attack2)
        {
            animator.SetTrigger("Attack2");
            attack2 = false;
        }
    }

    public void attackLock(Vector3 attackDirection, int attackType)
    {
        inAttack = true;
        moveDirection = transform.TransformDirection(attackDirection);

        if (attackType == 2)
        {
            moveDirection = transform.TransformDirection(new Vector3(xSpeed / 1.5f, 0f, attackDirection.z));
        }

    }

    public void attackUnlock()
    {
        inAttack = false;
        moveDirection = moveDirection = transform.TransformDirection(new Vector3(xSpeed, 0, zSpeed));
        localDirection = new Vector3(xSpeed, 0, zSpeed);
    }
}
