using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float HorizontalMovement = Input.GetAxis("Mouse X");
		float VerticalMovement = Input.GetAxis("Mouse Y");

		GetComponent<Rigidbody2D>().velocity = new Vector2(HorizontalMovement, VerticalMovement) * speed;
	}
}
