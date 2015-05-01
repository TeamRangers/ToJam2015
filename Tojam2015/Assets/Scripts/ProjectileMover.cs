using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(transform.forward * speed);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){


	}
}
