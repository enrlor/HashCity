using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Twitter {

	public class Timelines  {

			
		public static IEnumerator GetMentionsTimeline(AccessToken.AccessTokenResponse response, string lastTweet, Twitter.GetTweetListCallback callback){

			//Debug.Log ("GetMentionsTimeline entered");
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			if(!string.IsNullOrEmpty(lastTweet))
			   parameters.Add("since_id", lastTweet);
			
			return REST_API.GetTweetListRESTCall (TwitterUrls.REST_GET_MENTIONS_TIMELINE, response, parameters, callback);
			
		}

		public static IEnumerator GetHomeTimeline(AccessToken.AccessTokenResponse response, Twitter.GetTweetListCallback callback){
			
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			//parameters.Add("count", numMaxStatus.ToString());
			
			return REST_API.GetTweetListRESTCall (TwitterUrls.REST_GET_HOME_TIMELINE, response, parameters, callback);
			
		}


		public static IEnumerator GetUserTimeline(AccessToken.AccessTokenResponse response, Dictionary<string, string> p, Twitter.GetTweetListCallback callback){
			
			Dictionary<string, string> parameters = new Dictionary<string, string> ();
            //parameters.Add("count", numMaxStatus.ToString());
            foreach(KeyValuePair < string, string > elem in p){
                if (parameters.ContainsKey(elem.Key))
                    parameters[elem.Key] = elem.Value;
                else
                    parameters.Add(elem.Key, elem.Value);
            }

            return REST_API.GetTweetListRESTCall (TwitterUrls.REST_GET_USER_TIMELINE, response, parameters, callback);
			
		}

	}
}
