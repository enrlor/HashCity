using UnityEngine;
using System.Collections;
using Twitter;

public class CollisionDetector : MonoBehaviour {

	public GameObject particleEffectGood;
	public GameObject particleEffectBad;
	public GameObject currentTweetText;
	public int pointsToGet = 100;
	public int lifeToRestore = 10;

	void OnCollisionEnter(Collision other) {

		GameObject obj = other.gameObject;
		
		if (obj.tag.CompareTo ("TweetObstacle") != 0)
			return;
		
		Tweet tw =  obj.GetComponent<TweetObstacle> ().GetTweet();

		currentTweetText.GetComponent<UnityEngine.UI.Text>().text = tw.author.screen_name + "\n" + tw.text;

		if (obj.GetComponent<TweetObstacle> ().IsSpecial ()) {
		
			bool good = tw.hashtags_text.Contains ("bonus");
			string msg = (good) ? UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_BONUS_OK) : 
							UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_MALUS_OK);


			StartCoroutine (Twitter.Tweet.ReplyToTweet (UserPref.LoadTwitterUserInfo (), 
			                                        msg, tw.id, 
			                                        new Twitter.AckCallback (this.ReplyCallback)));  
		
			if (good) {

				GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ().RestoreHealt (lifeToRestore);
				particleEffectBad.SetActive (false);
				particleEffectGood.SetActive (true);

			} else {
			
				ScoreManager.score -= pointsToGet;
				particleEffectGood.SetActive (false);
				particleEffectBad.SetActive (true);

			}
		
			Invoke ("DisableParticleEffect", 1f);

			obj.GetComponent<TweetObstacle> ().Collided ();

			return;

		} else {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAnimationController> ().BeingHitted ();
		}

		SoundManager.Hurt ();

	}


	void ReplyCallback(bool success){

		Debug.Log ("reply " + success);

	}

	void DisableParticleEffect(){
	
		particleEffectGood.SetActive (false);
		particleEffectBad.SetActive (false);
	
	}

}

