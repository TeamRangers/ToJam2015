using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

	public string MouseX, MouseY;

	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float HorizontalMovement = Input.GetAxis(MouseX);
		float VerticalMovement = Input.GetAxis(MouseY);

		GetComponent<Rigidbody2D>().velocity = new Vector2(HorizontalMovement, VerticalMovement) * speed; //Move reticle based on the input
	}
}
