using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour{

	public int startingHealth = 100;                            // The amount of health the player starts the game with.
	public int currentHealth;                                   // The current health the player has.
	public Slider healthSlider;                                 // Reference to the UI's health bar.
	public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
	public AudioClip deathClip;                                 // The audio clip to play when the player dies.
	public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.


	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.


	void Awake(){
		currentHealth = startingHealth;	
	}


	void Update(){

		if (damaged) {
		
			damageImage.color = flashColour;
		
		} else {
		
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		
		}

		damaged = false;

	}


	public void TakeDamage (int amount){
	
		damaged = true;
		currentHealth -= amount;
		healthSlider.value = currentHealth;

		if(currentHealth <= 0 && !isDead)
			Death ();
	
	}

	public void RestoreHealt(int amount){
	
		currentHealth += amount;

		if (currentHealth > startingHealth)
			currentHealth = startingHealth;

		healthSlider.value = currentHealth;

	
	}

	void Death (){
	
		isDead = true;
	
	}


}