using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class UnityChanSimpleController : MonoBehaviour
{

	public float animSpeed = 1.5f;
	public float lookSmoother = 3.0f;			// a smoothing setting for camera motion
	// public bool useCurves = true;
	// public float useCurvesHeight = 0.5f;

	public float forwardSpeed = 1.2f;
	public float backwardSpeed = 0.5f;
	public float rotateSpeed = 0.3f;
	public float jumpPower = 4.0f; 
	public LayerMask stairLayer;
	private CapsuleCollider col;
	private Rigidbody rb;
	private Vector3 velocity;
	private float orgColHight;
	private Vector3 orgVectColCenter;
	private Animator anim;
	private AnimatorStateInfo currentBaseState;
	// private GameObject cameraObject;

	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	// static int restState = Animator.StringToHash ("Base Layer.Rest");

	void Start ()
	{
		anim = GetComponent<Animator> ();
		col = GetComponent<CapsuleCollider> ();
		rb = GetComponent<Rigidbody> ();
		// cameraObject = GameObject.FindWithTag ("MainCamera");
		orgColHight = col.height;
		orgVectColCenter = col.center;
	}


	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		anim.SetFloat("Speed", v);	
		anim.SetFloat("Direction", h);
		anim.speed = animSpeed;
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		rb.useGravity = true;
	
	
		velocity = new Vector3(0, 0, v);
		velocity = transform.TransformDirection(velocity);
		if (v > 0.1) {
			velocity *= forwardSpeed;
		} else if (v < -0.1) {
			velocity *= backwardSpeed;
		}
	
		if (Input.GetKeyDown("k")) {
			// Debug.Log("Button pressed");
			Debug.Log("want to jump");
			rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
			anim.SetTrigger("Jump");
		}
	
		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate (0, h * rotateSpeed, 0);
		
		if(currentBaseState.nameHash != jumpState && v > 0){
			Vector3 RayOrigin = new Vector3 (this.transform.position.x, this.transform.position.y , this.transform.position.z )
				+ this.transform.up * 2.0f + this.transform.forward;
			Ray ray = new Ray();
			ray.origin = RayOrigin;
			ray.direction = new Vector3 (0, -1, 0);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100.0f, stairLayer)){
				// Debug.Log(-this.transform.position.y + hit.point.y);
				Debug.Log("going up stair");
				this.transform.Translate(0, -this.transform.position.y + hit.point.y, Time.deltaTime * forwardSpeed);
			}
		}
	}

	public void damage(int num){
		Debug.Log("Get Damaged " + num.ToString());
	}

}
