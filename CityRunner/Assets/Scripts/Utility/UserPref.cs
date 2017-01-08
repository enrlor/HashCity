using UnityEngine;
using System.Collections;
using Twitter;

public class UserPref {

	// You need to save access token and secret for later use.
	// You can keep using them whenever you need to access the user's Twitter account. 
	// They will be always valid until the user revokes the access to your application.
	public static readonly string PLAYER_PREFS_TWITTER_USER_ID           = "TwitterUserID";
	public static readonly string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME  = "TwitterUserScreenName";
	public static readonly string PLAYER_PREFS_TWITTER_USER_TOKEN        = "TwitterUserToken";
	public static readonly string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

	public static readonly string PLAYER_PREFS_TEAM = "TwitterUserTeam";
	public static readonly string PLAYER_PREFS_LAST_SCORE = "TwitterUserLastScore";
	public static readonly string PLAYER_PREFS_LAST_MENTION_ID = "TwitterUserLastMentionID";

	public static readonly string PLAYER_PREFS_MSG_SCORE = "TwitterUser_MSG_SCORE";
	public static readonly string PLAYER_PREFS_MSG_BONUS_OK = "TwitterUser_MSG_BONUS_OK";
	public static readonly string PLAYER_PREFS_MSG_BONUS_NO = "TwitterUser_MSG_BONUS_NO";
	public static readonly string PLAYER_PREFS_MSG_MALUS_OK = "TwitterUser_MSG_MALUS_OK";
	public static readonly string PLAYER_PREFS_MSG_MALUS_NO = "TwitterUser_MSG_MALUS_NO";
	

	

	public static AccessToken.AccessTokenResponse LoadTwitterUserInfo(){

		AccessToken.AccessTokenResponse m_AccessTokenResponse = new AccessToken.AccessTokenResponse();
		
		m_AccessTokenResponse.UserId        = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_ID, null);
		m_AccessTokenResponse.ScreenName    = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_SCREEN_NAME, null);
		m_AccessTokenResponse.Token         = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN, null);
		m_AccessTokenResponse.TokenSecret   = PlayerPrefs.GetString(PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET, null);
	
		if (string.IsNullOrEmpty(m_AccessTokenResponse.Token) ||
		    string.IsNullOrEmpty(m_AccessTokenResponse.ScreenName) ||
		    string.IsNullOrEmpty(m_AccessTokenResponse.Token) ||
		    string.IsNullOrEmpty(m_AccessTokenResponse.TokenSecret))
		{
//			string log = "LoadTwitterUserInfo - succeeded";
//			log += "\n    UserId : " + m_AccessTokenResponse.UserId;
//			log += "\n    ScreenName : " + m_AccessTokenResponse.ScreenName;
//			log += "\n    Token : " + m_AccessTokenResponse.Token;
//			log += "\n    TokenSecret : " + m_AccessTokenResponse.TokenSecret;
//			Debug.Log(log);
			return null;
		}

		return m_AccessTokenResponse;

	}

	public static void AddPref(string key, string value){
	
		PlayerPrefs.SetString (key, value);
	
	}

	public static string GetPref(string key){
		
		return PlayerPrefs.GetString (key, null);
		
	}


	public static void Clear(){
	
		PlayerPrefs.DeleteAll ();
	
	}

}


