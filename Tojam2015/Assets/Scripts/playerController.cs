using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float aiAttackSpeed;
	private float nextAttackTime;

	public GameObject reticle;

	private Rigidbody2D rb;

	private IList<GameObject> enemies;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();

		if (activeAI){
			GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Player");
			enemies = new List<GameObject>();
			for (int i =0; i < enemiesArray.Length; i++){
				if (enemiesArray[i] != gameObject) {enemies.Add(enemiesArray[i]);}
			}
		}
	} 

	void FixedUpdate ()
	{
		if (activeAI){aiTick(); return;}
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
		foreach (GameObject enemy in enemies) {
			if (enemy.activeSelf){
				reticle.transform.position = enemy.transform.position;
				if (Time.time > nextAttackTime){
				FireProjectile();
					nextAttackTime = Time.time + aiAttackSpeed;
				}
				return;
			}
		}
	}
}