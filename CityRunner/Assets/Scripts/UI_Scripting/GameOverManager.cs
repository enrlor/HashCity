using UnityEngine;

public class GameOverManager : MonoBehaviour {
   
	public PlayerHealth playerHealth;       // Reference to the player's health.
	public float restartDelay = 5f;         // Time to wait before restarting the level
	
	
	Animator anim;                          // Reference to the animator component.
	GameManager gameManager;
	
	void Awake (){

		// Set up the reference.
		anim = GetComponent <Animator> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();

	}
	
	
	void Update (){

		// If the player has run out of health...
		if(playerHealth.currentHealth <= 0 && GameManager.gameState == GameManager.State.PLAYING){

			gameManager.GameOver();

			// ... tell the animator the game is over.
			anim.SetTrigger ("GameOver");

            GameObject.FindGameObjectWithTag("Player").GetComponent<ObstaclesGenerator>().SaveLastMentionID();

            UserPref.AddPref(UserPref.PLAYER_PREFS_LAST_SCORE, ScoreManager.score +"" );

			Invoke("GoToFinalStep", restartDelay);

			}
	}

	void GoToFinalStep(){
		
		Application.LoadLevel(Application.loadedLevel+1);
		
	}

}


