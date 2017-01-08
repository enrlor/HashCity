using UnityEngine;
using System.Collections;
using Twitter;

public class HideObstacle : MonoBehaviour {

	public float zLimit = 0f;

	void Update(){
	
		if (this.transform.position.z < zLimit) {

			if(this.GetComponent<TweetObstacle>().IsSpecial()){
				Tweet tw = this.GetComponent<TweetObstacle>().GetTweet();
				bool good = tw.hashtags_text.Contains("bonus");
				string msg = (good)? UserPref.GetPref(UserPref.PLAYER_PREFS_MSG_BONUS_NO) : UserPref.GetPref(UserPref.PLAYER_PREFS_MSG_MALUS_NO);
			
//				Debug.Log("SPECIAL " + ((good)? "GOOD" : "BAD"));
			
				StartCoroutine(Twitter.Tweet.ReplyToTweet(UserPref.LoadTwitterUserInfo(), 
			                                        msg, tw.id, 
				                                    new Twitter.AckCallback(this.ReplyCallback)));  
			}


			gameObject.SetActive (false);
		}
	}


	void ReplyCallback(bool success){
		
		Debug.Log ("reply " + success);
		
	}

}
