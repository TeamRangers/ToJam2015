﻿using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	public GameObject projectileObject;

	// Use this for initialization
	public float speed;
	public float jumpSpeed;

	public string Horizontal;
	public string Vertical;
	public string Fire;
	public string Jump;

	public bool activeAI;

	public GameObject reticle;

	private Rigidbody2D rb;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
	{
		if (activeAI){return aiTick();}
		float moveHorizontal = Input.GetAxis (Horizontal);
		float moveVertical = Input.GetAxis (Vertical);
		
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		
		rb.AddForce (movement * speed);
		//Vector2 v = rb.velocity;
		//v.x =  moveHorizontal * speed;
		//v.y = moveVertical * speed;
		//rb.velocity = v;
	}
	
	void Update ()
	{
		if (activeAI) {return;}
		bool down = Input.GetButtonDown(Jump);
		if (down) {
			rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
		}

		if (Input.GetButtonDown(Fire)){
			FireProjectile();

	}
}
	void FireProjectile(){
		Vector3 target = reticle.transform.position;
		target.z = 0;
		Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);
		Instantiate(projectileObject, Vector3.MoveTowards(transform.position, target, 1), projectileRotation);
	}

	void aiTick(){

	}
}