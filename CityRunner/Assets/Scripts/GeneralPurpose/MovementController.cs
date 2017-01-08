using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float speed = 0.0f;
	public Vector3 direction;
  

	// Update is called once per frame
	void Update () {
	
		transform.Translate (direction * speed);
		
	}
}
