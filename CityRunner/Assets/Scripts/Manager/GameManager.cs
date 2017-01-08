using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public enum State{
		STARTED = 0, //when the game in launched but is not playing
		PLAYING = 1,
		PAUSED = 2,
		GAME_OVER = 3 //gameover
	}

	public static State gameState = State.STARTED;
	public GameObject player;
	public ScoreManager scoreManager;
	public GameObject fixedBlockPath;
	public GameObject counter;
	public GameObject stopButton;
	public GameObject resumeButton;

	void Start(){
	
//		
//		StartGame ();
	
	}


	public void StartGame(){
	
		StartCoroutine ("Starting");
	
	}

	IEnumerator Starting(){

		counter.SetActive (true);
		counter.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "3";
		yield return new WaitForSeconds (1);
		counter.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "2";
		yield return new WaitForSeconds (1);
		counter.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "1";
		yield return new WaitForSeconds (0.3f);
		
		counter.SetActive (false);
		PlayerComponentSetActive (true);
	
		if(gameState == State.PAUSED)
			GameObject.FindGameObjectWithTag ("Player").GetComponent<GroundGenerator> ().Resume ();

		scoreManager.GameStarted ();
		yield return new WaitForSeconds (0.2f);
		SoundManager.StartRunning ();
		stopButton.SetActive (true);

		gameState = State.PLAYING;

	}

	public void PauseGame(){
	
		stopButton.SetActive (false);
		resumeButton.SetActive (true);
		PlayerComponentSetActive (false);
		scoreManager.GameStopped ();

		GameObject.FindGameObjectWithTag ("Player").GetComponent<GroundGenerator> ().Stop ();

		SoundManager.StopRunning ();

		gameState = State.PAUSED;
	
	}

	public void ResumeGame(){
	
		stopButton.SetActive (true);
		resumeButton.SetActive (false);

		StartCoroutine ("Starting");
	
	}

	public void GameOver(){
	
		PlayerComponentSetActive (false);
		scoreManager.GameStopped ();
		SoundManager.Die ();

		GameObject.Find ("CurrentTweet").SetActive (false);
		GameObject.Find ("ButtonCanvas").SetActive (false);
		GameObject.Find ("PlayerCollider").SetActive (false);


		gameState = State.GAME_OVER;
	
	}



	void PlayerComponentSetActive(bool active){

		player.GetComponent<PlayerAnimationController> ().enabled = active;
		player.GetComponent<GroundGenerator> ().enabled = active;
		player.GetComponent<ObstaclesGenerator> ().enabled = active;
		player.GetComponent<PlayerHealth> ().enabled = active;

		fixedBlockPath.SetActive (!active);

		if (active) {

			player.GetComponent<Animator>().CrossFade("ClaireRunning",0f);
		
		} else {

			player.GetComponent<Animator>().CrossFade("ClaireIdle",0f);
		}

		player.GetComponent<Animator>().SetBool("isRunning", active);

		//counter.SetActive (!active);

	}
}
