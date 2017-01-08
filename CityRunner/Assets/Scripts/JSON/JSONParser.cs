using System.Collections;
using System.Collections.Generic;
using LitJson;
using Twitter;
using UnityEngine;

public class JSONParser{
	
	
	public static Tweet ParseTweet(JsonData data){
		
		try{
			
			string tweetId = (string) data["id_str"];
			string tweetDate = (string) data["created_at"];
			string userDate = (string) data["user"]["created_at"];
			string text = (string) data["text"];
			string userId = (string) data["user"]["id_str"];
			string userName = (string) data["user"]["name"];
			string userScreenName = (string) data["user"]["screen_name"];
			string userDesc = (string) (data["user"]["description"] == null ? "" : data["user"]["description"]) ;
			int userFollowers = (int) data["user"]["followers_count"];
			int userFriends = (int) data["user"]["friends_count"];
			int userListed = (int) data["user"]["listed_count"];
			int userFav = (int) data["user"]["favourites_count"];
			int userStatuses = (int) data["user"]["statuses_count"];
			string userLang = (string) data["user"]["lang"];
			List<string> hash = new List<string>();
			List<string> userMentions = new List<string>();
			
			int j;
			int count;
			
			count = data ["entities"] ["hashtags"].Count;
			for(j=0; j<count; j++){
				hash.Add((string) data ["entities"] ["hashtags"][j]["text"]);
			}
			//Debug.Log(hash.Count);
			count = data ["entities"] ["user_mentions"].Count;
			for(j=0; j<count; j++){
				userMentions.Add((string) data ["entities"] ["user_mentions"][j]["screen_name"]);
			}
			
			TwitterUser user = new TwitterUser ( userId, userName, userScreenName, userDesc, userFollowers, userFriends, userListed, userFav, userStatuses, userDate, userLang);
			string status ;
			
			if(hash.Count == 1)
				status = "malus";
			else if (hash.Count > 1)
				status = "bonus";
			else
				status = "normal";
			
			//Debug.Log(status + " -> " + text);
			Tweet tt = new Tweet( tweetDate, tweetId, text, user, hash, userMentions);
		
			
			return tt;
			
		}catch(System.Exception){
			
			return null;
			
		}
		
	}
	
	public static List<Tweet> ParseTimeline(string encoded){
		
		List<Tweet> list = new List<Tweet> ();
		
		try{
			//(?)
			JsonData data = JsonMapper.ToObject(encoded);
			int countData = data.Count;
			for (int i = 0; i< countData; i++) {
				
				list.Add(ParseTweet(data[i]));
				
			}
			
			return list;
			
		}catch(KeyNotFoundException){
			
			return null;
			
		}
	}
	
	private static Warning ParseWarning(JsonData data){
		
		try{
			
			Warning w = new Warning( (string) data[0]["code"], (string) data[0]["message"], (int) data[0]["percent_full"]);
			return w;
			
		}catch (KeyNotFoundException){
			
			return null;
			
		}
		
	}
	
	public static System.Object Parse(string encoded){
		
		System.Object parsed;
		JsonData data = null;

		try{
			data = JsonMapper.ToObject(encoded);
		}catch(JsonException){
			Debug.LogError(encoded);
		}

		if (data == null)
			return null;
		else if (data.IsArray) {
			
			//(?)
			parsed = ParseTimeline (encoded);
			
			if(parsed != null){
				
				return parsed;
				
			}else{
				
				parsed = ParseWarning (data);
				
				if(parsed != null){
					
					return parsed;
					
				}
				
			}
			
		} else {
			
			parsed = ParseTweet (data);
			
			if(parsed != null){
				
				return parsed;
				
			}
			
		}
		
		return null;
		
	}
	

	public static TwitterUser ParseUser(string encoded){

		JsonData data = JsonMapper.ToObject(encoded);

		string userDate = (string) data["created_at"];
		string userId = (string) data["id_str"];
		string userName = (string) data["name"];
		string userScreenName = (string) data["screen_name"];
		string userDesc = (string) (data["description"] == null ? "" : data["description"]) ;
		int userFollowers = (int) data["followers_count"];
		int userFriends = (int) data["friends_count"];
		int userListed = (int) data["listed_count"];
		int userFav = (int) data["favourites_count"];
		int userStatuses = (int) data["statuses_count"];
		string userLang = (string) data["lang"];

		return new TwitterUser ( userId, userName, userScreenName, userDesc, userFollowers, userFriends, userListed, userFav, userStatuses, userDate, userLang);
	
	}

}