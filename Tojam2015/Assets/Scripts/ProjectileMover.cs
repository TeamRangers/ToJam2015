using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){


	}
}
