using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public GameObject[] typesOfObject;
	public int pooledAmount = 10;
	public int currentlyPooled = 0;
	public bool willgrow;
		
	List<GameObject> pooledObjects;
	
	void Awake() {

		pooledObjects = new List<GameObject>();
		for(int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate(typesOfObject[Random.Range(0,typesOfObject.Length)]);
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}

		currentlyPooled = pooledAmount;

	}
	
	public GameObject GetPooledObject() {

		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {

				return pooledObjects [i];
				
			}
		}

		if(willgrow){
	
			GameObject obj = (GameObject) Instantiate(typesOfObject[Random.Range(0,typesOfObject.Length)]);
			pooledObjects.Add(obj);
			currentlyPooled++; 
			return obj;
		
		}
		
		return null;
	
	}
	
		

}
