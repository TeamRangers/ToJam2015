using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int startHealth = 100;
	public int currentHealth;

	bool isDead;

	void Awake () {
		currentHealth = startHealth;
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

		if (currentHealth <= 0 && !isDead) {
			Death();
		}
	}

	void Death() {
		isDead = true;

		gameObject.SetActive(false);
	}
}
