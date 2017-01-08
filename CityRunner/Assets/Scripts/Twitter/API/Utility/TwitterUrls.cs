using UnityEngine;
using System.Collections;

namespace Twitter {

	public class TwitterUrls {

		public static readonly string USER_STREAM  = "https://userstream.twitter.com/1.1/user.json";
		public static readonly string FILTER_STREAM  = "https://stream.twitter.com/1.1/statuses/filter.json";
		public static readonly string SAMPLE_STREAM = "https://stream.twitter.com/1.1/statuses/sample.json";


		public static readonly string REST_GET_USER = "https://api.twitter.com/1.1/users/show.json";

		public static readonly string REST_POST_TWEET = "https://api.twitter.com/1.1/statuses/update.json";
		public static readonly string REST_SEARCH_TWEET = "https://api.twitter.com/1.1/search/tweets.json";
		public static readonly string REST_RETWEET = "https://api.twitter.com/1.1/statuses/retweet/:id.json";

		public static readonly string REST_DIRECT_MESSAGE = "https://api.twitter.com/1.1/direct_messages/new.json";

		public static readonly string REST_GET_MENTIONS_TIMELINE = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";
		public static readonly string REST_GET_HOME_TIMELINE = "https://api.twitter.com/1.1/statuses/home_timeline.json";
		public static readonly string REST_GET_USER_TIMELINE = "https://api.twitter.com/1.1/statuses/user_timeline.json";


	}

}