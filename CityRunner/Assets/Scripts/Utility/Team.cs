using UnityEngine;
using System.Collections;

public class Team  {

	public string id { get; set; }
	public string name { get; set; }
	public string screen_name { get; set; }
	public int points { get; set; }
	public bool pro { get; set; }

	public Team(){}

	public Team (string id, string name, string screen_name, int points, bool pro){
	
		this.id = id;
		this.name = name;
		this.screen_name = screen_name;
		this.points = points;
		this.pro = pro;
	
	}

}

