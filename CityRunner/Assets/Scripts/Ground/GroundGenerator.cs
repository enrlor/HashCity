using UnityEngine;
using System.Collections;

public class GroundGenerator : MonoBehaviour {
	
	public float pieceLenght;
	public Vector3 direction;
	
	private GameObject pathPool;
	
	private GameObject current;
	private GameObject previous;
	private GameObject next;
	
	// Use this for initialization
	void Start () {
		
		pathPool = GameObject.FindGameObjectWithTag ("GroundPool");

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
		next.transform.Translate (direction * pieceLenght);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (gameObject.transform.position.z > (current.transform.position.z + (pieceLenght/2))) {
			
			if(previous!=null)
				previous.SetActive(false);
			previous = current;
			
			current = next;
			
			next = pathPool.GetComponent<ObjectPooler> ().GetPooledObject ();
			next.SetActive (true);
			next.transform.position = current.transform.position;
			next.transform.Translate (direction * pieceLenght);
		}
		
	}

	public GameObject GetCurrentPath(){
	
		return current;
	
	}

	public void Stop(){
	
		current.GetComponent<MovementController> ().enabled = false;
		next.GetComponent<MovementController> ().enabled = false;
	
		if(previous!=null)
			previous.GetComponent<MovementController> ().enabled = false;

	}


	public void Resume(){
	
		current.GetComponent<MovementController> ().enabled = true;
		next.GetComponent<MovementController> ().enabled = true;

		if(previous!=null)
			previous.GetComponent<MovementController> ().enabled = true;

	}
}
