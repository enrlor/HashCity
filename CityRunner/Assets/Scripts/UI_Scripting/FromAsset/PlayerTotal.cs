using UnityEngine;
using System.Collections;

public class PlayerTotal : MonoBehaviour {
	

	public float pieceLenght= 26f;

	public Transform mainCamera;
	public float dispacement = 0.05f;
	public float smoothTime = 0.2f;
	
	public float obstaclesIntervall = 0.2f;
	public float howMuchFar = 2.0f;



	private Vector3 rightPosition;
	private Vector3 leftPosition;
	private Vector3 centerPosition;
	private Vector3 myPosition;
	private Vector3 velocity = Vector3.zero;
	
	private float offsetY;
	private float offsetZ;

	
	private GameObject pathPool;
	private GameObject obstaclesPool;
		
	private GameObject current;
	private GameObject previous;
	private GameObject next;

	private Animator anim;

	void Awake(){
		
		offsetY = mainCamera.transform.position.y;
		offsetZ = mainCamera.transform.position.z;
		
	}


	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		rightPosition = GameObject.FindGameObjectWithTag ("Right").transform.position;
		leftPosition = GameObject.FindGameObjectWithTag ("Left").transform.position;
		centerPosition = GameObject.FindGameObjectWithTag ("Center").transform.position;
		myPosition = centerPosition;


		pathPool = GameObject.FindGameObjectWithTag ("GroundPool");
		
		InitGround ();

		obstaclesPool = GameObject.FindGameObjectWithTag ("ObstaclesPool");
		
		InvokeRepeating ("PlaceObstacle", 0f, obstaclesIntervall);
	
	}


	// Update is called once per frame
	void Update () {
	
		CheckAnimations ();
		BuildGround ();
		BeFollowed ();

	}

	private void InitGround(){
	
		if (pathPool == null) {
			Debug.Log("Prolem");
		}
		
		current = pathPool.GetComponent<ObjectPooler> ().GetPooledObject ();
		
		if (current == null) {
			Debug.Log ("NULL from start");
			return;
		}
		
		current.SetActive (true); 
		
		previous = null;
		
		next = pathPool.GetComponent<ObjectPooler> ().GetPooledObject ();
		next.SetActive (true);
		next.transform.position = current.transform.position;
		next.transform.Translate (Vector3.left * pieceLenght);
	
	}

	private void BuildGround(){

		if (gameObject.transform.position.z > (current.transform.position.z + pieceLenght)) {
			
			if(previous!=null)
				previous.SetActive(false);
			previous = current;
			
			current = next;
			
			next = pathPool.GetComponent<ObjectPooler> ().GetPooledObject ();
			next.SetActive (true);
			next.transform.position = current.transform.position;
			next.transform.Translate (Vector3.left * pieceLenght);
		}

	}

	private void CheckAnimations(){

		if (Input.GetKeyDown ("right")) {
			
			Vector3 temp = this.transform.position;
			
			if (myPosition == leftPosition)
				temp.x = centerPosition.x;
			else
				temp.x = rightPosition.x;
			
			this.transform.position = temp;
			myPosition.x = temp.x;
			
		}
		
		if (Input.GetKeyDown ("left")) {
			
			Vector3 temp = this.transform.position;
			
			if (myPosition == rightPosition)
				temp.x = centerPosition.x;
			else
				temp.x = leftPosition.x;
			
			this.transform.position = temp;
			myPosition.x = temp.x;
		}
		
		if (Input.GetKeyDown ("up")) {
			
			anim.SetTrigger ("Jump");
			
		}
		
		if (Input.GetKeyDown ("down")) {
			
			anim.SetTrigger ("Slide");
			
		}
	
	}


	private void BeFollowed(){
	
		Vector3 targetPosition;
		
		if (this.transform.position.x == centerPosition.x)
			targetPosition = new Vector3 (centerPosition.x, 
			                              transform.position.y + offsetY, 
			                              transform.position.z + offsetZ);
		
		else if (this.transform.position.x == leftPosition.x)
			targetPosition = new Vector3 (leftPosition.x + dispacement, 
			                              transform.position.y + offsetY, 
			                              transform.position.z + offsetZ);
		
		else //if (this.transform.position.x == rightPosition.x)
			targetPosition = new Vector3 (rightPosition.x - dispacement, 
			                              transform.position.y + offsetY, 
			                              transform.position.z + offsetZ);
		
		mainCamera.transform.position = Vector3.SmoothDamp (mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
		//mainCamera.transform.position = targetPosition;
	
	}

	private void PlaceObstacle (){
		
		GameObject obj = obstaclesPool.GetComponent<ObjectPooler> ().GetPooledObject ();
		
		obj.SetActive (true);
		
		Vector3 obstaclePosition = new Vector3 (0f, 
		                                        obj.transform.position.y, 
		                                        this.transform.position.z + howMuchFar);
		
		obj.transform.position = obstaclePosition;
		
		
	}
}
