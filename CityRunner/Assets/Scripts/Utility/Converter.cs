using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Converter {

	public static string BuildStringParameters(Dictionary<string,string> d){

		if (d.Count == 0)
			return "";

		string s = "";
		
		foreach(KeyValuePair<string, string> entry in d)
		{
			s = s + entry.Key + "=" + WWW.EscapeURL(entry.Value, System.Text.Encoding.UTF8) + "&";
		}
		
		if (s [s.Length - 1] == '&') {
			
			s = s.Substring (0, s.Length-1);
			
		}

		return s;
		
	}

}
