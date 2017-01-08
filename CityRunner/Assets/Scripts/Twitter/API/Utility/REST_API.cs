using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitter {

	public class REST_API  {

		public static IEnumerator PostRESTCall(string url, AccessToken.AccessTokenResponse response, Dictionary<string, string> parameters, Twitter.AckCallback callback){

			// Add data to the form to post.
			WWWForm form = new WWWForm();
			
			foreach(KeyValuePair<string,string> par in parameters){
				
				form.AddField(par.Key, par.Value);
				
			}

			// HTTP header
			var headers = new Dictionary<string, string>();
			headers["Authorization"] = AccessToken.GetHeaderWithAccessToken("POST", 
			                                                                url, 
			                                                                UserPref.LoadTwitterUserInfo(), 
			                                                                parameters);
			
			WWW web = new WWW(url, form.data, headers);
			yield return web;
			
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("PostRESTCall - failed. {0}\n{1}", web.error, web.text));
				callback(false);
			}
			else
			{
				string error = Regex.Match(web.text, @"<error>([^&]+)</error>").Groups[1].Value;
				
				if (!string.IsNullOrEmpty(error))
				{
					Debug.Log(string.Format("PostRESTCall - failed. {0}", error));
					callback(false);
				}
				else
				{
					callback(true);
				}
			}

		}


		public static IEnumerator GetRESTCall (string url, AccessToken.AccessTokenResponse response, Dictionary<string, string> parameters, Twitter.GetJSONCallback callback){
			
			string stringParams = Converter.BuildStringParameters (parameters);
			
			url = (!string.IsNullOrEmpty (stringParams)) ? url + "?" + stringParams : url;
			//Debug.Log (url);
		
			// HTTP header
			var headers = new Dictionary<string, string>();
			headers ["Authorization"] = AccessToken.GetHeaderWithAccessToken ("GET", 
			                                                                  url,  
			                                                                  UserPref.LoadTwitterUserInfo (), 
			                                                                  parameters);
			
			WWW web = new WWW(url, null, headers);
			yield return web;
			
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetRESTCall - failed. {0}\n{1}", web.error, web.text));
				callback(false, null);
			}
			else
			{
				string error = Regex.Match(web.text, @"<error>([^&]+)</error>").Groups[1].Value;
				
				if (!string.IsNullOrEmpty(error))
				{
					Debug.Log(string.Format("GetRESTCall - failed. {0}", error));
					callback(false, null);
				}
				else
				{
					callback(true, web.text);
				}
			}
			
		}


		public static IEnumerator GetTweetListRESTCall (string url,AccessToken.AccessTokenResponse response, Dictionary<string, string> parameters, Twitter.GetTweetListCallback callback){

			string stringParams = Converter.BuildStringParameters (parameters);
			
			url = (!string.IsNullOrEmpty (stringParams)) ? url + "?" + stringParams : url;

			// HTTP header
			var headers = new Dictionary<string, string>();
			headers ["Authorization"] = AccessToken.GetHeaderWithAccessToken ("GET", 
			                                                                  url,  
			                                                                  UserPref.LoadTwitterUserInfo (), 
			                                                                  parameters);

			WWW web = new WWW(url, null, headers);
			yield return web;
			
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetTweetListRESTCall - failed. {0}\n{1}", web.error, web.text));
				callback(false, null);
			}
			else
			{
				string error = Regex.Match(web.text, @"<error>([^&]+)</error>").Groups[1].Value;
				
				if (!string.IsNullOrEmpty(error))
				{
					Debug.Log(string.Format("GetTweetListRESTCall - failed. {0}", error));
					callback(false, null);
				}
				else
				{
					List<Tweet> list = JSONParser.ParseTimeline(web.text);
					callback(true, list);
				}
			}
			
		}

	}

}
