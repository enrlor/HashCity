using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

	private static Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();

	public static string RUN_SOUND = "Footsteps";
	public static string LAND_SOUND = "Jump_Land";
	public static string DEATH_SOUND = "Death";
	public static string HURT_SOUND = "Hurt";
	public static string SLIDE_SOUND = "Slide";
	public static string CLICK_SOUND = "Button";
	public static string CHECK_SOUND = "CheckButton";
	public static string BG_MUSIC = "music";
		

	void Start(){
	

		AudioSource[] s = this.GetComponents<AudioSource> ();

		for (int i = 0; i < s.Length ; i++) {

			if(!sources.ContainsKey(s[i].clip.name))
				sources.Add(s[i].clip.name, s[i]);
			else
				sources[s[i].clip.name] = s[i];

		} 

	}

	public static void StartRunning(){
	
		if(!sources.ContainsKey(RUN_SOUND))
			return;

		sources [RUN_SOUND].Play ();
		sources [RUN_SOUND].loop = true;

	}

	public static void StopRunning(){
		
		sources [RUN_SOUND].Stop ();

		
	}

	public static void JumpLand(float delay){

		if(!sources.ContainsKey(LAND_SOUND))
			return;

		sources [RUN_SOUND].Stop ();
		sources [LAND_SOUND].PlayDelayed (delay);
		sources [RUN_SOUND].PlayDelayed (delay + sources [LAND_SOUND].clip.length );

	}

	public static void Slide(){

		if(!sources.ContainsKey(SLIDE_SOUND))
			return;

		sources [RUN_SOUND].Stop ();
		sources [SLIDE_SOUND].Play ();
		sources [RUN_SOUND].PlayDelayed ( sources [SLIDE_SOUND].clip.length );
		
	}


	public static void Hurt(){

		if(!sources.ContainsKey(HURT_SOUND))
		   return;

		sources [RUN_SOUND].Stop ();
		sources [HURT_SOUND].Play ();
		sources [RUN_SOUND].PlayDelayed ( sources [HURT_SOUND].clip.length );

		
	}


	public static void Die(){

		StopRunning ();
		sources [BG_MUSIC].Stop ();

		if(sources.ContainsKey(DEATH_SOUND))
			sources [DEATH_SOUND].Play ();

	}


	public void Click(){

		if(sources.ContainsKey(CLICK_SOUND))
			sources [CLICK_SOUND].Play ();

	}

	public void Check(){
		 
		if(sources.ContainsKey(CHECK_SOUND))
			sources [CHECK_SOUND].Play ();
		
	}


}
