using UnityEngine;
using System.Collections;
using Twitter;

public class TwitterLogin : MonoBehaviour {


	RequestToken.RequestTokenResponse m_RequestTokenResponse;
	AccessToken.AccessTokenResponse m_AccessTokenResponse;
	GuiUtility.Callback PIN_requesterCallback;


	// Use this for initialization
	void Awake () {
		
		m_AccessTokenResponse = UserPref.LoadTwitterUserInfo();
		
	}
	

	public string GetScreenName(){
	
		string text = string.Empty;

		if (m_AccessTokenResponse == null)
			return text;
	
		if (!string.IsNullOrEmpty (m_AccessTokenResponse.ScreenName)) {
			text = m_AccessTokenResponse.ScreenName;
		} 
	
		return text;

	}


	public void Logout(){
	
		UserPref.Clear();
	
	}


	public void Login(){
	
		StartCoroutine (Twitter.Authorization.GetRequestToken (new RequestToken.RequestTokenCallback 
		                                             (this.OnRequestTokenCallback)));

	}


	public void checkPIN(string m_PIN, GuiUtility.Callback callback){

		StartCoroutine (Twitter.Authorization.GetAccessToken (m_RequestTokenResponse.Token, m_PIN,
		                                   new AccessToken.AccessTokenCallback (this.OnAccessTokenCallback)));

		PIN_requesterCallback = callback;
	}


	
	void OnRequestTokenCallback(bool success, RequestToken.RequestTokenResponse response)
	{
		if (success)
		{
			string log = "OnRequestTokenCallback - succeeded";
			log += "\n    Token : " + response.Token;
			log += "\n    TokenSecret : " + response.TokenSecret;
			print(log);
			
			m_RequestTokenResponse = response;
			
			Twitter.Authorization.OpenAuthorizationPage(response.Token);
		}
		else
		{
			print("OnRequestTokenCallback - failed.");
		}
	}


	void OnAccessTokenCallback(bool success, AccessToken.AccessTokenResponse response) {

		if (success){

			string log = "OnAccessTokenCallback - succeeded";
			log += "\n    UserId : " + response.UserId;
			log += "\n    ScreenName : " + response.ScreenName;
			log += "\n    Token : " + response.Token;
			log += "\n    TokenSecret : " + response.TokenSecret;
			print(log);
			
			UserPref.AddPref(UserPref.PLAYER_PREFS_TWITTER_USER_ID, response.UserId);
			UserPref.AddPref(UserPref.PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, response.ScreenName);
			UserPref.AddPref(UserPref.PLAYER_PREFS_TWITTER_USER_TOKEN, response.Token);
			UserPref.AddPref(UserPref.PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, response.TokenSecret);

			m_AccessTokenResponse = UserPref.LoadTwitterUserInfo();

		}
		else{
			print("OnAccessTokenCallback - failed.");
		}

		PIN_requesterCallback(success);

	}
	
}
