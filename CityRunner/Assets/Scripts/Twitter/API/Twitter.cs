
using System.Collections.Generic;

namespace Twitter {

    /// <summary>
    /// This delegate function identify the operation to perform as callback for a Twitter API which does not return something (ie PostTweet, etc). 
    /// </summary>
    /// <param name="success">Boolean parameter, set to true if the API request was successful, false otherwise</param>
	public delegate void AckCallback(bool success);

    /// <summary>
    /// This delegate function identify the operation to perform as callback for a Twitter API which returns a list of Tweets (ie SearchAPI, Timelines, etc). 
    /// </summary>
    /// <param name="success">Boolean parameter, set to true if the API request was successful, false otherwise</param>
    /// <param name="list">List of Tweets returned by the API request (if successful)</param>
	public delegate void GetTweetListCallback(bool success, List<Tweet> list);

    /// <summary>
    /// This delegate function identify the operation to perform as callback for a Twitter API which returns an object (ie GetTwitterUser, etc). 
    /// </summary>
    /// <param name="success">Boolean parameter, set to true if the API request was successful, false otherwise</param>
    /// <param name="jsonEncoded">The json representation of the object returned by the call (if successful)</param>
	public delegate void GetJSONCallback(bool success, string jsonEncoded);

}