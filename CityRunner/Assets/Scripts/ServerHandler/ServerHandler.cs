using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Twitter;

public class ServerHandler : MonoBehaviour{

	public string server = "http://www.enricaloria.altervista.org/HashCity/server.php";
	
	public delegate void ServerRequestCallback(bool success, List<Team>  list);



	public void GetAllTeams(ServerRequestCallback callback){
	
		WWWForm form = new WWWForm();
		form.AddField("GetTeams", "true");
		
		//print out data returned here
		WWW result = new WWW (server, form.data);
		
		StartCoroutine(WaitForRequest(result, callback));

	
	}

	public void GetAllTeams(ServerRequestCallback callback, bool ranking){
		
		WWWForm form = new WWWForm();
		form.AddField("GetTeams", "true");
		form.AddField("ranking", ranking+"");
		
		//print out data returned here
		WWW result = new WWW (server, form.data);
		
		StartCoroutine(WaitForRequest(result, callback));
		
		
	}




	public void AddTeam (Team team, ServerRequestCallback callback){

		WWWForm form = new WWWForm();
		form.AddField("AddTeam", "true");
		form.AddField("name", team.name);
		form.AddField("id", team.id);
		form.AddField("screen_name", team.screen_name);

		//print out data returned here
		WWW result = new WWW (server, form.data);

		StartCoroutine(WaitForRequest(result,callback));

	}

		
	public void LoadPoints (string id, int points, bool against, ServerRequestCallback callback){
		
		WWWForm form = new WWWForm();
		form.AddField("LoadPoints", "true");
		form.AddField("id", id);
		form.AddField("points", points + "");
		form.AddField("against", against + "");

		//print out data returned here
		WWW result = new WWW (server, form.data);
		
		StartCoroutine(WaitForRequest(result, callback));
		
	}


	private IEnumerator WaitForRequest(WWW www, ServerRequestCallback callback){
		
		yield return www;
			
		if (www.error == null){

			JsonData data = JsonMapper.ToObject (www.text);

			if (((int)data ["error"]) == 0){

				if(data.Keys.Contains("teams")){

					List<Team> teams = new List<Team>();
					
					for (int i = 0; i < data["teams"].Count; i++) {
						
						string name = (string)data ["teams"][i] ["name"];
						string id = (string)data ["teams"][i] ["id"];
						string screen_name = (string)data ["teams"][i] ["screen_name"];
						int points = int.Parse((string) data ["teams"][i] ["points"]);
						
						teams.Add (new Team(id, name, screen_name, points, true));
						
					}
					
					callback (true, teams);

				}else
					callback (true, null);
			
			}else
				callback (false, null);

		} else {
		
			callback(false, null);
		
		}    

	}

}
