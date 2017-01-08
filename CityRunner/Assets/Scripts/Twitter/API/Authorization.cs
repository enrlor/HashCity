using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitter {

	public class Authorization {

		// Twitter APIs for OAuth process
		private static readonly string RequestTokenURL = "https://api.twitter.com/oauth/request_token";
		private static readonly string AuthorizationURL = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";
		private static readonly string AccessTokenURL = "https://api.twitter.com/oauth/access_token";

		public static IEnumerator GetRequestToken(RequestToken.RequestTokenCallback callback) {
			WWW web = WWWRequestToken();
			
			yield return web;
			
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetRequestToken - failed. error : {0}", web.error));
				callback(false, null);
			}
			else
			{
				RequestToken.RequestTokenResponse response = new RequestToken.RequestTokenResponse
				{
					Token = Regex.Match(web.text, @"oauth_token=([^&]+)").Groups[1].Value,
					TokenSecret = Regex.Match(web.text, @"oauth_token_secret=([^&]+)").Groups[1].Value,
				};
				
				if (!string.IsNullOrEmpty(response.Token) &&
				    !string.IsNullOrEmpty(response.TokenSecret))
				{
					callback(true, response);
				}
				else
				{
					Debug.Log(string.Format("GetRequestToken - failed. response : {0}", web.text));
					
					callback(false, null);
				}
			}
		}

		public static IEnumerator GetAccessToken(string requestToken, string pin, AccessToken.AccessTokenCallback callback)
		{
			WWW web = WWWAccessToken(requestToken, pin);
			
			yield return web;
			
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetAccessToken - failed. error : {0}", web.error));
				callback(false, null);
			}
			else
			{
				AccessToken.AccessTokenResponse response = new AccessToken.AccessTokenResponse
				{
					Token = Regex.Match(web.text, @"oauth_token=([^&]+)").Groups[1].Value,
					TokenSecret = Regex.Match(web.text, @"oauth_token_secret=([^&]+)").Groups[1].Value,
					UserId = Regex.Match(web.text, @"user_id=([^&]+)").Groups[1].Value,
					ScreenName = Regex.Match(web.text, @"screen_name=([^&]+)").Groups[1].Value
				};
				
				if (!string.IsNullOrEmpty(response.Token) &&
				    !string.IsNullOrEmpty(response.TokenSecret) &&
				    !string.IsNullOrEmpty(response.UserId) &&
				    !string.IsNullOrEmpty(response.ScreenName))
				{
					callback(true, response);
				}
				else
				{
					Debug.Log(string.Format("GetAccessToken - failed. response : {0}", web.text));
					
					callback(false, null);
				}
			}
		}

		private static WWW WWWRequestToken()
		{
			// Add data to the form to post.
			WWWForm form = new WWWForm();
			form.AddField("oauth_callback", "oob");
			
			// HTTP header
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			OauthConnection.AddDefaultOAuthParams(parameters);
			parameters.Add("oauth_callback", "oob");
			
			var headers = new Dictionary<string, string>();
			headers["Authorization"] = OauthConnection.GetFinalOAuthHeader("POST", RequestTokenURL, parameters);
			
			return new WWW(RequestTokenURL, form.data, headers);
		}
		
		private static WWW WWWAccessToken(string requestToken, string pin)
		{
			// Need to fill body since Unity doesn't like an empty request body.
			byte[] dummmy = new byte[1];
			dummmy[0] = 0;
			
			// HTTP header
			var headers = new Dictionary<string, string>();
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			OauthConnection.AddDefaultOAuthParams(parameters);
			parameters.Add("oauth_token", requestToken);
			parameters.Add("oauth_verifier", pin);
			
			headers["Authorization"] = OauthConnection.GetFinalOAuthHeader("POST", AccessTokenURL, parameters);
			
			return new WWW(AccessTokenURL, dummmy, headers);
		}

		public static void OpenAuthorizationPage(string requestToken)
		{
			Application.OpenURL(string.Format(AuthorizationURL, requestToken));
		}
	}

}
