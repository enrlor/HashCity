using UnityEngine;
using System.Collections;

public class TeamOption : MonoBehaviour {
	
	private Team team;
	
	
	public void SetTeam(Team t){
		
		this.team = t;
		
	}
	
	public Team GetTeam(){
		
		return team;
		
	}

}
