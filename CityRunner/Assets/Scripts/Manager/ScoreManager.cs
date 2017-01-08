using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour{
	
	public static int score;        // The player's score.
	public float scoreIntervall;
	
	Text text;                      // Reference to the Text component.
	
	
	void Awake (){

		// Set up the reference.
		text = GetComponent <Text> ();
		
		// Reset the score.
		score = 0;

	}
	
	
	public void GameStarted(){
		
		InvokeRepeating ("IncreaseScore", scoreIntervall, scoreIntervall);
		
	}


	public void GameStopped(){
		
		CancelInvoke ();
		
	}

	
	void IncreaseScore(){
		
		score += 1;
		text.text = "Score: " + score;
		
	}
	
}
