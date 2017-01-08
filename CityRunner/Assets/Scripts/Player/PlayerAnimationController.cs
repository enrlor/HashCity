using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour {

	public float timeOfMovement = 1.5f;

	Animator anim;
	int jumpHash = Animator.StringToHash("ClaireJump");
	int slideHash = Animator.StringToHash("ClaireSlide");
	int hittedHash = Animator.StringToHash("ClaireStumble");
	int rightStepHash = Animator.StringToHash("ClaireRightStep");
	int leftStepHash = Animator.StringToHash("ClaireLeftStep");
	int runStateHash = Animator.StringToHash("Base Layer.ClaireRunning");
	
	private Vector3 rightPosition;
	private Vector3 leftPosition;
	private Vector3 centerPosition;
	private Vector3 myPosition;
	private Vector3 colliderOriginalPosition;

	private bool colliderIsRotated = false;
	private bool alreadyMoving = false;

	private GameObject playerCollider;
	

	void Start () {

		anim = GetComponent<Animator>();

		rightPosition = GameObject.FindGameObjectWithTag ("Right").transform.position;
		leftPosition = GameObject.FindGameObjectWithTag ("Left").transform.position;
		centerPosition = GameObject.FindGameObjectWithTag ("Center").transform.position;
		playerCollider = GameObject.FindGameObjectWithTag ("PlayerCollider");

		myPosition = centerPosition;
		colliderOriginalPosition = playerCollider.transform.position;
	}


	void Update ()
	{

		AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		Vector3 newColliderPos = playerCollider.transform.position;
		Vector3 targetPosition = this.transform.position;; 

		MOVEMENT mov = GetMovement ();

//		Debug.Log (mov);

		if ( (Input.GetKeyDown (KeyCode.UpArrow) || mov == MOVEMENT.UP )
		    							&& stateInfo.fullPathHash == runStateHash) {
			
			//anim.SetTrigger (jumpHash);
			anim.CrossFade(jumpHash, 0f);
			newColliderPos.y += 0.13f;

			float length = anim.GetCurrentAnimatorStateInfo(0).length;
			SoundManager.JumpLand(length);
			StartCoroutine("ResetCollider", length);
			
		}
		
		else if ( (Input.GetKeyDown (KeyCode.DownArrow) || mov == MOVEMENT.DOWN ) 
		         							&& stateInfo.fullPathHash == runStateHash) {
			
			//anim.SetTrigger (slideHash);
			anim.CrossFade(slideHash, 0f);

			newColliderPos.y = 0.03f;
			
			playerCollider.transform.Rotate ( new Vector3(90f, 0f,0f));
			colliderIsRotated = true;

			SoundManager.Slide();
			StartCoroutine("ResetCollider", anim.GetCurrentAnimatorStateInfo(0).length);
			
		}

		else if ( (Input.GetKeyDown (KeyCode.RightArrow) || mov == MOVEMENT.RIGHT ) 
		         														&& !alreadyMoving) {

			anim.CrossFade(rightStepHash, 0f);

			Vector3 temp = this.transform.position;

			if(myPosition == leftPosition)
				temp.x = centerPosition.x;
			else
				temp.x = rightPosition.x;

			targetPosition = temp;

			myPosition.x = temp.x;

			StartCoroutine(MoveFromTo(this.transform, this.transform.position, targetPosition, timeOfMovement));

		}

		else if ( (Input.GetKeyDown (KeyCode.LeftArrow) || mov == MOVEMENT.LEFT ) 
		         														&& !alreadyMoving) {

			anim.CrossFade(leftStepHash, 0f);

			Vector3 temp = this.transform.position;

			if(myPosition == rightPosition)
				temp.x = centerPosition.x;
			else
				temp.x = leftPosition.x;

			myPosition.x = temp.x;

			targetPosition = temp;

			StartCoroutine(MoveFromTo(this.transform, this.transform.position, targetPosition, timeOfMovement));

		}


		newColliderPos.x = myPosition.x;
		playerCollider.transform.position = newColliderPos;

	 	//this.transform.position = Vector3.MoveTowards (this.transform.position, targetPosition, 0.4f);
//		if(!alreadyMoving)
//			StartCoroutine(MoveFromTo(this.transform, this.transform.position, targetPosition, timeOfMovement));
//		//this.transform.position = targetPosition;
	}

	IEnumerator MoveFromTo(Transform objectToMove, Vector3 a, Vector3 b, float speed) {

		float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
		float t = 0;
		alreadyMoving = true;

		while (t <= 1.0f) {
			t += step; // Goes from 0 to 1, incrementing by step each time
			objectToMove.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
			yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
		}

		alreadyMoving = false;
		objectToMove.position = b;

	}



	private IEnumerator  ResetCollider(float lenght){
	
		//Debug.Log (anim.runtimeAnimatorController.animationClips [index].length);
		//wait for the animation to stop and then adjust the collider
		yield return new WaitForSeconds(lenght); //this parameter should be the lenght of the animation, but it has problems with 1sec...
		playerCollider.transform.position = colliderOriginalPosition;

		if (colliderIsRotated) {

			playerCollider.transform.Rotate (new Vector3 (270f, 0f, 0f));
			colliderIsRotated = false;
	
		}

	}


	public void BeingHitted(){
	
		//anim.SetTrigger (hittedHash);
		anim.CrossFade(hittedHash, 0f);
		this.GetComponent<PlayerHealth> ().TakeDamage (1);
	}


	//inside class
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;
	
	private MOVEMENT GetMovement() {

		if(Input.touches.Length > 0) {

			Touch t = Input.GetTouch(0);

			if(t.phase == TouchPhase.Began) {
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}

			if(t.phase == TouchPhase.Ended) {
				//save ended touch 2d point
				secondPressPos = new Vector2(t.position.x,t.position.y);
				
				//create vector from the two points
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
				//normalize the 2d vector
				currentSwipe.Normalize();
				
				//swipe upwards
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					return MOVEMENT.UP;
				}
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					return MOVEMENT.DOWN;
				}
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f &&  currentSwipe.y < 0.5f){
					return MOVEMENT.LEFT;
				}
				//swipe right
				if(currentSwipe.x > 0 &&  currentSwipe.y > -0.5f &&  currentSwipe.y < 0.5f){
					return MOVEMENT.RIGHT;
				}
			}
		}

		return MOVEMENT.NONE;

	}


	private enum MOVEMENT{
		UP = 1,
		DOWN = 2,
		LEFT = 3,
		RIGHT = 4,
		NONE = 0	
	}


}
