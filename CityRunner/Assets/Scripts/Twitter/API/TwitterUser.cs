using System;
using System.Globalization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Twitter {

    public enum GetUserBy
    {
        SCREEN_NAME = 0,
        USER_ID = 1
    }

    /// <summary>
    /// This class represent an User of Twitter. 
    /// It contains a subset of the informations available: id, name, screen name, description, 
    /// counts (followers, friends, listed, favourites, statuses), date of the account creation, user preferred language.
    /// </summary>

	public class TwitterUser: UnityEngine.Object {

		public string id { get; private set; }
		new public string name { get; private set; }
		public string screen_name { get; private set; }
		public string description { get; private set; }
		public int followers_count { get; private set; }
		public int friends_count { get; private set; }
		public int listed_count { get; private set; }
		public int favourites_count { get; private set; }
		public int statuses_count { get; private set; }
		public string creationDate { get; private set; }
		public string language { get; private set; }

		public TwitterUser (){
		
			//do nothing
		
		}

		public TwitterUser (string i, string n, string s, string d, int followers, int friends,
		                    int listed, int fav, int status, string date, string lang){
		
			this.id = i;
			this.name = n;
			this.screen_name = s;
			this.description = d;
			this.followers_count = followers;
			this.friends_count = friends;
			this.listed_count = listed;
			this.favourites_count = fav;
			this.statuses_count = status;
			this.language = lang;
			this.creationDate = DateTime.ParseExact(date,
			                                        "ddd MMM dd HH:mm:ss zzz yyyy",
			                                        CultureInfo.InvariantCulture,
			                                        DateTimeStyles.AdjustToUniversal).ToShortDateString();
		}


		public override string ToString(){
		
			return "[Date = " + creationDate/*.ToShortDateString () */+ ", ID = " + id + ", ScreenName = " + screen_name + ", Description = " + description
						+ ", Followers = " + followers_count + ", Friends = " + friends_count + ", Listed = " + listed_count + ", Favourites = " + favourites_count
							+ ", Statuses = " + statuses_count + ", Language = " + language + "]";

		}

		/// <summary>
        /// Get the User specified by screen name or id.
        /// </summary>
        /// <param name="response">Access Token which identifies the request</param>
        /// <param name="paramType">Type of the following parameters, which specifies if the user have to be researched by screen name or id.</param>
        /// <param name="param">Value of the research parameter</param>
        /// <param name="callback">Callback function that must be called after the API request.</param>
		public static IEnumerator GetUser(AccessToken.AccessTokenResponse response, GetUserBy paramType, string param, Twitter.GetJSONCallback callback)
		{
			if (string.IsNullOrEmpty(param))
			{
				Debug.Log(string.Format("GetUser - text[{0}] is empty.", param));
				
				callback(false, null);
				return null;
			}
			else
			{
				if(param.StartsWith("@"))
					param = param.Substring(1);

				Dictionary<string, string> parameters = new Dictionary<string, string>();
                
                if (paramType == GetUserBy.SCREEN_NAME)
                    parameters.Add("screen_name", param);
                else if (paramType == GetUserBy.USER_ID)
                    parameters.Add("user_id", param);
                else
                {
                    Debug.Log(string.Format("GetUser - text[{0}] is empty.", paramType));
                    callback(false, null);
                    return null;
                }


                return REST_API.GetRESTCall(TwitterUrls.REST_GET_USER, response, parameters, callback);
				
			}
		}

	}

}
