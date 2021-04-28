//
// Mecanimのアニメーションデータが、原点で移動しない場合の Rigidbody付きコントローラ
// サンプル
// 2014/03/13 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

namespace UnityChan
{
// 必要なコンポーネントの列記
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
		public float jumpPower = 3.0f; 
        public LayerMask stairLayer;
		private CapsuleCollider col;
		private Rigidbody rb;
		private Vector3 velocity;
		private float orgColHight;
		private Vector3 orgVectColCenter;
		private Animator anim;
		private AnimatorStateInfo currentBaseState;
		// private GameObject cameraObject;

		static int idleState = Animator.StringToHash("Base Layer.Idle");
		static int locoState = Animator.StringToHash("Base Layer.Locomotion");
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
	
	
		void Update ()
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
		
			if (Input.GetButtonDown("Jump")) {
                // Debug.Log("Button pressed");
				if (currentBaseState.nameHash != jumpState) {
					// Debug.Log("able to jump");
                    if (!anim.IsInTransition(0)) {
						rb.AddForce (Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool ("Jump", true);
					}
				}
			}
		

			transform.localPosition += velocity * Time.fixedDeltaTime;
			transform.Rotate (0, h * rotateSpeed, 0);	

		    if (currentBaseState.nameHash == jumpState) {
				if (!anim.IsInTransition(0)) {		
					anim.SetBool("Jump", false);
				}
			}

            
            if(v > 1e-5f){
                Vector3 RayOrigin = new Vector3 (this.transform.position.x, this.transform.position.y , this.transform.position.z )
                    + this.transform.up + this.transform.forward * forwardSpeed;
                Ray ray = new Ray();
                ray.origin = RayOrigin;
                ray.direction = new Vector3 (0, -1, 0);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0f, stairLayer)){
					// Debug.Log(-this.transform.position.y + hit.point.y);
                	this.transform.Translate(0, -this.transform.position.y + hit.point.y, Time.deltaTime * forwardSpeed);
				}
            }
		}

	}
}