using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	// Use this for initialization
	public float speed;
	public float jumpSpeed;
	
	private Rigidbody2D rb;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		
		Vector2 movement = new Vector2 (moveHorizontal, 0.0f);
		
		//rb.AddForce (movement * speed);
		Vector2 v = rb.velocity;
		v.x =  moveHorizontal * speed;
		rb.velocity = v;
	}

	void Update ()
	{
		bool down = Input.GetButtonDown("Jump");
		if (down) {
			rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
		}
	}
}
