using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RobotPlayer))]
public class PlayerHealth : MonoBehaviour {

	public int startHealth = 100;
	public int currentHealth;
	public Slider healthSlider;

    RobotPlayer _player;
	bool _isDead;

	void Start () {
		currentHealth = startHealth;
		healthSlider.value = startHealth;
        _player = GetComponent<RobotPlayer>();
	}	

	public void TakeDamage (int damage)
	{
		currentHealth = Mathf.Max(currentHealth - damage, 0);

		healthSlider.value = currentHealth;

		if (currentHealth <= 0 && !_isDead) {
			Death();
		}

	}

	void Death() {
		_isDead = true;
        _player.Die();
	}

}
