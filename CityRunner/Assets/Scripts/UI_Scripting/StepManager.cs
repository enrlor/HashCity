using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Twitter;

public class StepManager : MonoBehaviour {
	
	private TwitterLogin tw;
	private int currentStep = 0;
	private Team chosenTeam;
	private bool logged = false;

	public GameObject[] windows;

	public GameObject loginLabel;
	public GameObject pinText;

	public GameObject teamLabel;
	public GameObject teamText;

	public GameObject teamsMenu;
	public GameObject teamsList;
	public GameObject optionPrefab;

	public GameObject settingsMenu;

	public GameObject teamSelectedText;

	public GameObject serverHandler;

	public GameObject txtmsgScore;
	public GameObject txtmsgBonusOK;
	public GameObject txtmsgBonusNO;
	public GameObject txtmsgMalusOK;
	public GameObject txtmsgMalusNO;

	AccessToken.AccessTokenResponse m_AccessTokenResponse = null;


	// Use this for initialization
	void Start () {
	
		tw = GetComponent<TwitterLogin> ();

		string text = tw.GetScreenName ();

		if (!string.IsNullOrEmpty (text)) {

			logged = true;
			loginLabel.GetComponent<Text>().text = text + "\nClick to change account";
			m_AccessTokenResponse = UserPref.LoadTwitterUserInfo();

		}
	}
	


	public void OnLoginClick(){

		tw.Login ();
	
	}

	public void OnLogoutClick(){
		
		tw.Logout ();
		loginLabel.GetComponent<Text>().text = "Login to Twitter";
		m_AccessTokenResponse = null;
		logged = false;
		
	}


	public void OnInsertPin(){
	
		string pin = pinText.GetComponent<Text> ().text;
		tw.checkPIN (pin, new GuiUtility.Callback(this.OnInsertPinCallback));
	
	}

	private void OnInsertPinCallback(bool success){

		if (!success)
			return;

		string text = tw.GetScreenName ();
		Debug.Log (text);
		if (!string.IsNullOrEmpty (text)) {
			loginLabel.GetComponent<Text>().text = text;
			logged = true;
			
		}

	}


	public void OnInsertTeam(){
	
		string name = teamText.GetComponent<Text> ().text;

        teamText.GetComponent<Text>().text = "";
        if (string.IsNullOrEmpty (name))
			return;

		StartCoroutine(Twitter.TwitterUser.GetUser(m_AccessTokenResponse, GetUserBy.SCREEN_NAME, name, 
		                                   new Twitter.GetJSONCallback(this.OnInsertTeamCallback)));
	
	}
		              
	void OnInsertTeamCallback(bool success, string encodeduser){

		if (success && !string.IsNullOrEmpty(encodeduser)) {
		
			TwitterUser user = (TwitterUser) JSONParser.ParseUser(encodeduser);

			Team team = new Team (user.id, user.name, user.screen_name, 0, true);
			serverHandler.GetComponent<ServerHandler> ()
				.AddTeam (team, new ServerHandler.ServerRequestCallback (this.AfterServerRequest));
			
			teamLabel.GetComponent<Text> ().text = "You're playing for: " + team.name;
            
            chosenTeam = team;
		
		} else {
		
			teamText.GetComponent<Text> ().text = "User don't exists or has limits";
		
		}

	}
		             


	public void OnChooseTeam(){
	
		serverHandler.GetComponent<ServerHandler> ()
			.GetAllTeams (new ServerHandler.ServerRequestCallback(this.AfterServerRequest));

	
	}

	public void OnSelectTeam(BaseEventData ev){
	

		Team selectedTeam = ev.selectedObject.GetComponent<TeamOption> ().GetTeam(); 

		string msg = "<screen_name>\nName: <name>\nScore: <score>";
		msg = msg.Replace("<screen_name>", "@" + selectedTeam.screen_name);
		msg = msg.Replace("<name>", selectedTeam.name);
		msg = msg.Replace("<score>", selectedTeam.points + "");

		teamSelectedText.GetComponent<Text> ().text = msg;

		this.gameObject.GetComponent<SoundManager> ().Check ();

	}

	public void GoBack(){

		windows[currentStep].SetActive(false);
		windows[currentStep-1].SetActive(true);

		currentStep--;

	}


	public void GoAhead(){
	
		switch (currentStep) {
		
		case 0:
			if(!logged || (UserPref.LoadTwitterUserInfo() == null))
				return;
			break;
		
		case 1:
			if((chosenTeam == null) || !logged)
				return;
			break;
		
		}

		windows[currentStep].SetActive(false);
		currentStep++;
		windows[currentStep].SetActive(true);

	}


	void AfterServerRequest(bool success, List<Team>  list){
		
		if (!success) {
			
			Debug.Log ("Problem");
			
		} else {
			
			Debug.Log ("Everything okay");
			if(list == null) //it has been added a new team
				return ;

			ShowTeamList(list);
				
		}
		
	}

//	void Update(){
//	
//		if (teamsMenu.activeInHierarchy)
//			OnSelectTeam ();
//	
//	}


	void ShowTeamList(List<Team>  list){
	
		Vector3 pos = new Vector3(80f, -10f, 0f);

		foreach(Team element in list){ 

			GameObject obj = Instantiate(optionPrefab);

			obj.GetComponent<TeamOption>().SetTeam(element);
			obj.transform.SetParent(teamsList.transform, false);
			obj.GetComponent<Toggle>().group = teamsList.GetComponent<ToggleGroup>();

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Select;
			entry.callback.AddListener(this.OnSelectTeam);
			obj.AddComponent<EventTrigger>().triggers.Add(entry);

			//set the text to the team name
			obj.transform.Find("Label").gameObject.GetComponent<Text>().text = element.name;

			obj.GetComponent<RectTransform>().anchoredPosition3D = pos;

			pos.y -= 30f;

		}

		teamsMenu.SetActive (true);
		windows [currentStep].SetActive (false);

	}

	public void HideTeamList (){
	
		IEnumerable<Toggle> active = teamsList.GetComponent<ToggleGroup> ().ActiveToggles ();
	
		IEnumerator<Toggle> aa = active.GetEnumerator (); 

		//we are sure that we have only one active toggle
		aa.MoveNext ();
		chosenTeam = aa.Current.gameObject.GetComponent<TeamOption> ().GetTeam();

		active = GameObject.Find ("SideToggleGroup").GetComponent<ToggleGroup> ().ActiveToggles ();
		aa = active.GetEnumerator (); 
		aa.MoveNext ();

		if (aa.Current.gameObject.transform.Find ("Label").gameObject.GetComponent<Text> ().text == "Pro")
			chosenTeam.pro = true;
		else
			chosenTeam.pro = false;

		teamLabel.GetComponent<Text> ().text = "You're playing " +
			(chosenTeam.pro ? "for" : "against")
			+ ": " + chosenTeam.name;

		teamsMenu.SetActive (false);
		windows [currentStep].SetActive (true);
	
	}

	public void OnPlay(){
	
        //check if msg are null in this case add default messages.
//		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_SCORE, txtmsgScore.GetComponent<Text>().text);
//		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_BONUS_OK, txtmsgBonusOK.GetComponent<Text>().text);
//		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_BONUS_NO, txtmsgBonusNO.GetComponent<Text>().text);
//		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_MALUS_OK, txtmsgMalusOK.GetComponent<Text>().text);
//		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_MALUS_NO, txtmsgMalusNO.GetComponent<Text>().text);

		UserPref.AddPref(UserPref.PLAYER_PREFS_TEAM, LitJson.JsonMapper.ToJson(chosenTeam));

		Application.LoadLevel (1);
	}

	public void OpenSettingsMenu(){	

		txtmsgScore.GetComponent<Text>().text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_SCORE);
		txtmsgBonusOK.GetComponent<Text>().text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_BONUS_OK);
		txtmsgBonusNO.GetComponent<Text>().text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_BONUS_NO);
		txtmsgMalusOK.GetComponent<Text>().text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_MALUS_OK);
		txtmsgMalusNO.GetComponent<Text>().text = UserPref.GetPref (UserPref.PLAYER_PREFS_MSG_MALUS_NO);

		windows [currentStep].transform.parent.gameObject.SetActive (false);
		settingsMenu.SetActive (true);
	
	}


	public void CloseSettingsMenu(){

		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_SCORE, txtmsgScore.GetComponent<Text>().text);
		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_BONUS_OK, txtmsgBonusOK.GetComponent<Text>().text);
		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_BONUS_NO, txtmsgBonusNO.GetComponent<Text>().text);
		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_MALUS_OK, txtmsgMalusOK.GetComponent<Text>().text);
		UserPref.AddPref (UserPref.PLAYER_PREFS_MSG_MALUS_NO, txtmsgMalusNO.GetComponent<Text>().text);

		settingsMenu.SetActive (false);
		windows [currentStep].transform.parent.gameObject.SetActive (true);

	}

}
