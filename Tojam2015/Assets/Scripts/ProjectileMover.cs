using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float speed;
	public int damage = 20;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(transform.forward * speed, ForceMode2D.Impulse);
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.CompareTag("Player")) {
			PlayerHealth playerHealth = coll.gameObject.GetComponent<PlayerHealth>();
			if (playerHealth.currentHealth > 0) {
				playerHealth.TakeDamage(damage);
			}
		}
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){


	}
}
