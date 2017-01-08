using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Twitter;

public class FinalScoreManager : MonoBehaviour {

	public GameObject text;
	public GameObject postTweetButton;
	public GameObject postTweetLabel;
	public GameObject mainMenu;
	public GameObject rankingMenu;
	public GameObject rankingText;
	public ServerHandler serverHandler;
	
	string score ;
	string screen_name;
	Team team;

	AccessToken.AccessTokenResponse m_AccessTokenResponse;

	// Use this for initialization
	void Awake () {

		m_AccessTokenResponse = UserPref.LoadTwitterUserInfo ();

		screen_name = UserPref.GetPref(UserPref.PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
		team = LitJson.JsonMapper.ToObject<Team>(UserPref.GetPref(UserPref.PLAYER_PREFS_TEAM));
		score = UserPref.GetPref(UserPref.PLAYER_PREFS_LAST_SCORE);

		string message = text.GetComponent<Text> ().text;

		message = message.Replace ("<screen_name>", screen_name);
		message = message.Replace ("<score>", (team.pro ? "" : "-" ) + score);
		text.GetComponent<Text> ().text = message;

		//load points
		serverHandler.LoadPoints(team.id, int.Parse(score), 
		                         ((team.pro) ? false : true), 
		                         new ServerHandler.ServerRequestCallback(this.OnLoadPointsCallback));

	}
	

	public void PostTweet () {
	
		string text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_SCORE);

		text = text.Replace("<score>", score+"");
		text = text.Replace ("<teamname>", team.name);

		string msg = text + "!!! #hashcity #playforanidea";
		StartCoroutine(Twitter.Tweet.PostTweet (m_AccessTokenResponse, msg, null, 
		                                      new Twitter.AckCallback (this.Callback)));

	}

	void Callback(bool success){

		if (success) {
			postTweetButton.GetComponent<Button>().interactable = false;
			postTweetLabel.GetComponent<Text>().color = Color.green;
		}

	}

	void OnLoadPointsCallback(bool success, List<Team>  list){

		if (success) {

			text.GetComponent<Text> ().color = Color.green;

		} else {
		
			text.GetComponent<Text> ().color = Color.red;
		
		}

	}

	public void Ranking(){
	
		serverHandler.GetAllTeams (new ServerHandler.ServerRequestCallback(this.OnRankingCallback), true);
	
	}


	public void Back(){
		
		mainMenu.SetActive(true);
		rankingMenu.SetActive(false);

	}


	void OnRankingCallback(bool success, List<Team>  list){
	
		if (success) {
		
			mainMenu.SetActive(false);
			rankingMenu.SetActive(true);

			string teamsRanking = "";
			foreach(Team t in list){
			
				teamsRanking += ">> " + t.name + "   " + t.points + " points\n";
			
			}

			rankingText.GetComponent<UnityEngine.UI.Text>().text = teamsRanking;
		
		}
	
	}
	

}
