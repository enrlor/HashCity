using UnityEngine;
using System.Collections;
using Twitter;

public class TweetObstacle : MonoBehaviour {
	

	private Tweet tweet;
	private bool isSpecial;


	public void SetTweet(Tweet t, bool special){
		
		this.tweet = t;
		this.isSpecial = special;
		
	}
	
	public Tweet GetTweet(){
		
		return tweet;
		
	}
	
	public bool IsSpecial(){

		return isSpecial;

	}

	public void Collided(){

		this.isSpecial = false;
	
	}
}
