using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace Twitter {

    /// <summary>
    /// This class represents a Tweet entity. It collects a subset of the informations available (creation date, id, text, author,
    /// list of hashtags and list of user mentions.
    /// </summary>
	public class Tweet {

		public string creationDate { get; private set; }
		public string id { get; private set; }
		public string text { get; private set; }
		public TwitterUser author { get; private set; }
		public List<string> hashtags_text { get; private set; }
		public List<string> user_mentions { get; private set; }
		
		public Tweet (){
			
			//do nothing
			
		}
		
		public Tweet ( string d, string i, string t, TwitterUser user, List<string> h, List<string> u) {
			
			DateTime dt = DateTime.ParseExact(d,
			                                  "ddd MMM dd HH:mm:ss zzz yyyy",
			                                  CultureInfo.InvariantCulture,
			                                  DateTimeStyles.AdjustToUniversal);
			
			this.creationDate = dt.ToShortDateString();
			this.id = i;
			this.text = t;
			this.author = user;
			this.hashtags_text = h;
			this.user_mentions = u;
			
		}
		
		public override string ToString(){
			string s =  "[Date = " + creationDate + 
				", ID = " + id + 
					", Text = " + text + 
					", Author = " + author.ToString() + ", Hashtags = [";
			
			int cont = hashtags_text==null ? 0 : hashtags_text.Count ;
			
			for(int i=0; i < cont; i++){
				s += " " +hashtags_text[i];
			}
			
			s += " ], User Mentions = [";
			
			cont = user_mentions==null ? 0 : user_mentions.Count;
			for(int i=0; i < cont; i++){
				s += " " +user_mentions[i];
			}
			
			s += "]]";
			
			return s;
		}

        /// <summary>
        /// Post a tweet with the specified text with the autenticated user as publisher. Other optional parameters can be inserted. 
        /// See Twitter API documentation for more.
        /// </summary>
        /// <param name="response">Access Token Response retrieved in the authentication process</param>
        /// <param name="text">Text to publish</param>
        /// <param name="optionalParam">Optional parameters for the Post Tweet API. See the documentation for the keys value available.</param>
        /// <param name="callback">Function called after the process is finished</param>
		public static IEnumerator PostTweet(AccessToken.AccessTokenResponse response,string text, Dictionary<string,string> optionalParam, Twitter.AckCallback callback)
		{
			if (string.IsNullOrEmpty (text) || text.Length > 140) {
				Debug.Log (string.Format ("PostTweet - text[{0}] is empty or too long.", text));
				
				callback (false);
				return null;
			} else {
				Dictionary<string, string> parameters = new Dictionary<string, string> ();
				parameters.Add ("status", text);
                
                if(optionalParam!=null)
                    foreach(KeyValuePair<string,string> pair in optionalParam)
                    {
                        parameters.Add(pair.Key, pair.Value);
                    }

				return REST_API.PostRESTCall (TwitterUrls.REST_POST_TWEET, response, parameters, callback);
			}
		}

        /// <summary>
        /// Post a reply to the tweet with the specified id with the autenticated user as publisher.
        /// See Twitter API documentation for more.
        /// </summary>
        /// <param name="response">Access Token Response retrieved in the authentication process</param>
        /// <param name="text">Text to publish</param>
        /// <param name="tweetID">ID of the tweet that has to be retweeted</param>
        /// <param name="callback">Function called after the process is finished</param>
        /// <returns></returns>
		public static IEnumerator ReplyToTweet(AccessToken.AccessTokenResponse response, string text, string tweetID, Twitter.AckCallback callback)
		{

			Dictionary<string, string> parameters = new Dictionary<string, string> ();
			parameters.Add ("in_reply_to_status_id", tweetID);

            return PostTweet(response, text, parameters , callback);

		}

        /// <summary>
        /// Post a retweet of the tweet with the specified id with the autenticated user as publisher. 
        /// See Twitter API documentation for more.
        /// </summary>
        /// <param name="response">Access Token Response retrieved in the authentication process</param>
        /// <param name="id">ID of the tweet that has to be retweeted</param>
        /// <param name="callback">Function called after the process is finished</param>
		public static IEnumerator Retweet(AccessToken.AccessTokenResponse response, string id, Twitter.AckCallback callback)
		{
			if (string.IsNullOrEmpty(id))
			{
				Debug.Log(string.Format("Id - text[{0}] is empty", id));
				
				callback(false);
				return null;
			}
			else
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("id", id);

                return REST_API.PostRESTCall (TwitterUrls.REST_RETWEET, response, parameters, callback);

			}
		}

        /// <summary>
        /// Search all the tweets that responds at the specified query. Other optional parameters can be inserted. 
        /// See Twitter API documentation for more. 
        /// </summary>
        /// <param name="response">Access Token Response retrieved in the authentication process</param>
        /// <param name="query">A UTF-8, URL-encoded search query of 500 characters maximum, including operators. Queries may additionally be limited by complexity.</param>
        /// <param name="optionalParam">Optional parameters for the Search Tweets API. See the documentation for the keys value available.</param>
        /// <param name="callback">Function called after the process is finished</param>
		public static IEnumerator SearchTweets(AccessToken.AccessTokenResponse response, string query, Dictionary<string, string> optionalParam, Twitter.GetJSONCallback callback)
		{
			if (string.IsNullOrEmpty(query) || query.Length > 500)
			{
				Debug.Log(string.Format("Query - text[{0}] is empty or too long.", query));
				
				callback(false, null);
				return null;
			}
			else
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add ("q", query);

                if (optionalParam != null)
                    foreach (KeyValuePair<string, string> pair in optionalParam)
                    {
                        parameters.Add(pair.Key, pair.Value);
                    }

                return REST_API.GetRESTCall (TwitterUrls.REST_SEARCH_TWEET, response, parameters, callback);
				
			}
		}

	}
}
