using UnityEngine;
using System.Collections.Generic;
using LitJson;
using Twitter;

public class ObstaclesGenerator : MonoBehaviour {

	private static readonly string PUBLIC_TWEET = "public";
	private static readonly string GOOD_TAG = "bonus";
	private static readonly string BAD_TAG = "malus";

	public GameObject obstaclesPool;
	public GameObject bonusPool;
	public GameObject malusPool;

	private string lastMentionID;

	public float publicTweetsIntervall = 0.7f;
	public float specialTweetsIntervall = 5f;
	public float streamReadingIntervall = 0.1f;
	public float howMuchFar = 2.0f;
    public float specialDistance = 2.0f;
    public int queueMaxSize = 1000;

	private Vector3[] positions;
	private string todayDate;
	private string screenname;

	AccessToken.AccessTokenResponse m_AccessTokenResponse;
	StreamingChannel publicChannel;

	Queue<Tweet> specialTweets = new Queue<Tweet>();
	List<Tweet> publicTweets = new List<Tweet>();

	float timeElapsed = 0f;

    void Awake() {

        LoadTweetFromTimeline();

    }

	// Use this for initialization
	void Start () {

		todayDate = System.DateTime.Now.Year + "-" + 
				System.DateTime.Now.Month + "-" + 
				System.DateTime.Now.Day ;
		
		screenname = UserPref.GetPref (UserPref.PLAYER_PREFS_TWITTER_USER_SCREEN_NAME);
        lastMentionID = UserPref.GetPref(UserPref.PLAYER_PREFS_LAST_MENTION_ID);

		if(!screenname.StartsWith("@"))
		   screenname = "@" + screenname;

		positions = new Vector3[3];
		positions[0] = GameObject.FindGameObjectWithTag ("Right").transform.position;
		positions[1] = GameObject.FindGameObjectWithTag ("Left").transform.position;
		positions[2] = GameObject.FindGameObjectWithTag ("Center").transform.position;

		m_AccessTokenResponse = UserPref.LoadTwitterUserInfo();

		//publicChannel = new Twitter.StreamingChannel(m_AccessTokenResponse, null);

	}


	Vector3 publicX;

	private void PlaceTweet (){
	
		//Dictionary<string,Tweet> receivedTweets = GetTweetFromStream ();

		Dictionary<string,Tweet> tweetsToRender = new Dictionary<string,Tweet>();
        GameObject obj;

        if (publicTweets.Count > 0) {

            Tweet t = publicTweets[0];
            publicTweets.RemoveAt(0);
            publicTweets.Add(t);
            tweetsToRender.Add(PUBLIC_TWEET, t);

        }

		if(specialTweets.Count>0){
			
			Tweet tweet = specialTweets.Dequeue();
			//Debug.Log("Count = " + specialTweets.Count + " -- " + tweet.ToString()); 
			if(tweet.hashtags_text.Contains(GOOD_TAG))
				tweetsToRender.Add(GOOD_TAG, tweet);
			else if(tweet.hashtags_text.Contains(BAD_TAG))
				tweetsToRender.Add(BAD_TAG, tweet);
			
		}


		if (tweetsToRender == null || tweetsToRender.Count == 0)
			return;


		if (tweetsToRender.ContainsKey (PUBLIC_TWEET)) {

            obj = PickFromPool(obstaclesPool.GetComponent<ObjectPooler>(), TweetType.NORMAL);
            if(obj != null)
                    obj.GetComponent<TweetObstacle> ().SetTweet (tweetsToRender [PUBLIC_TWEET], false);

		}
		if (tweetsToRender.ContainsKey (GOOD_TAG)) {

            obj = PickFromPool(bonusPool.GetComponent<ObjectPooler>(), TweetType.SPECIAL_GOOD);
            if (obj != null)
                    obj.GetComponent<TweetObstacle> ().SetTweet (tweetsToRender [GOOD_TAG], true);
		
		}

        if (tweetsToRender.ContainsKey(BAD_TAG)) {

            obj = PickFromPool(malusPool.GetComponent<ObjectPooler>(), TweetType.SPECIAL_BAD);
            if (obj != null)
                obj.GetComponent<TweetObstacle>().SetTweet(tweetsToRender[BAD_TAG], true);

        }
	

	}


	private void GetTweetFromQuery(){
        //		Debug.Log (screenname + " #hashcity bonus OR malus since:" + todayDate);
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("since_id", lastMentionID);
		StartCoroutine(Twitter.Tweet.SearchTweets(m_AccessTokenResponse,
		                                       screenname + " #hashcity bonus OR malus since:" + todayDate,
		                                       param,
		                                       new Twitter.GetJSONCallback(this.OnSearchTweetCallback)));
		
	}


	void OnSearchTweetCallback (bool success, string jsonResponse){
	
		if (success) {

			JsonData data = JsonMapper.ToObject (jsonResponse);

			if (data ["statuses"].IsArray) {
			
				int countData = data ["statuses"].Count;

				for (int i = 0; i< countData; i++) {
					
					Tweet t = JSONParser.ParseTweet (data ["statuses"] [i]);

//					Debug.Log("QUERY " + t.ToString());

					if ((t.hashtags_text.Contains ("hashcity")) && 
					    		((t.hashtags_text.Contains ("bonus")) || (t.hashtags_text.Contains ("malus")))) {

						specialTweets.Enqueue (t);
						if(string.IsNullOrEmpty(lastMentionID) || (t.id.CompareTo(lastMentionID) > 0))
							lastMentionID = t.id;

					}

				}
			
			}
		

		} else
			Debug.Log("OnSearchTweetCallback - failed.");

	}


	private GameObject PickFromPool(ObjectPooler pool, TweetType type){
	
		GameObject obj = pool.GetPooledObject ();
        GameObject currPath = this.GetComponent<GroundGenerator>().GetCurrentPath();

        if (obj == null || currPath == null)
            return null;

		obj.SetActive (true);
        
		obj.transform.SetParent (currPath.transform);

		Vector3 tempX = Vector3.zero;
        float zPosition = this.transform.position.z;

        if (type == TweetType.NORMAL) {

            tempX = positions[Random.Range(0, positions.Length)];
            publicX = tempX;
            zPosition += howMuchFar;

        } else if (type == TweetType.SPECIAL_BAD) {

            zPosition += specialDistance;
            tempX = this.transform.position;

        } else if (type == TweetType.SPECIAL_GOOD) {

            zPosition += specialDistance;
            /*tempX = positions[0];
            tempX.x = this.transform.position.x;
            int i = System.Array.IndexOf(positions, tempX);
            tempX = positions[(i + 1) % positions.Length];*/
       
        }

        Vector3 obstaclePosition = new Vector3 (tempX.x, 
		                                        obj.transform.position.y, 
		                                        zPosition);

		obj.transform.position = obstaclePosition;

//		Debug.Log (obj.name + " " + obj.transform.position);
		return obj;
	
	}

	public GameObject alert;
 

    void ConnectionProblems(){
	
		alert.SetActive (true);

		GameObject manager = GameObject.Find ("GameManager");
		manager.GetComponent<GameManager> ().PauseGame ();
	
	
	}

    public void SaveLastMentionID() {

        UserPref.AddPref(UserPref.PLAYER_PREFS_LAST_MENTION_ID, lastMentionID);

    }

    private void LoadTweetFromTimeline() {

        Team team = LitJson.JsonMapper.ToObject<Team>(UserPref.GetPref(UserPref.PLAYER_PREFS_TEAM));

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("user_id", team.id);
        parameters.Add("count", 200+"");

        StartCoroutine(Twitter.Timelines.GetUserTimeline(m_AccessTokenResponse, parameters, 
            new Twitter.GetTweetListCallback(this.OnLoadTweetsCallback)));

    }

    private bool connected = false;
    void OnLoadTweetsCallback(bool success, List<Tweet> list) {

        connected = success;
        if (success)  {

            publicTweets = list;
            Debug.Log(publicTweets.Count);
            this.OnEnable();

        } else {
            ConnectionProblems();
        }

    }

    void OnDisable(){
	
		CancelInvoke ();
	
	}


	void OnEnable(){

        if (connected) {

            InvokeRepeating("PlaceTweet", 0f, publicTweetsIntervall);

            //search api rate limit is 180 request per 15 minutes = 1 request each 
            //5 secs but i do a request each 10 secs for now
            InvokeRepeating("GetTweetFromQuery", specialTweetsIntervall, specialTweetsIntervall);

        }

	}


    private enum TweetType
    {
        SPECIAL_GOOD = 1,
        SPECIAL_BAD = 2,
        NORMAL = 0
    }

}
