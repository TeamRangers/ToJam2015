using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startHealth = 100;
	public int currentHealth;
	public Slider healthSlider;

	bool isDead;

	void Awake () {
		currentHealth = startHealth;
		healthSlider.value = startHealth;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage (int amount)
	{
		currentHealth -= amount;
		healthSlider.value = currentHealth;
		if (currentHealth <= 0 && !isDead) {
			Death();
		}
	}

	void Death() {
		isDead = true;

		gameObject.SetActive(false);
		GetComponent<playerController>().reticle.gameObject.SetActive(false);
	}

}
