using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startHealth = 100;
	public int currentHealth;
	public Slider healthSlider;

	bool isDead;

	void Start () {
		currentHealth = startHealth;
		healthSlider.value = startHealth;
	}	

	public void TakeDamage (int damage)
	{
		currentHealth = Mathf.Max(currentHealth - damage, 0);

		healthSlider.value = currentHealth;

		if (currentHealth <= 0 && !isDead) {
			Death();
		}

	}

	void Death() {
		isDead = true;
		gameObject.SetActive(false);		
	}

}
