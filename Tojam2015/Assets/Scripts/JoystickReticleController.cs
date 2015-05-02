using UnityEngine;
using System.Collections;

public class JoystickReticleController : MonoBehaviour {

	public string Horizontal, Vertical;
	public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = new Vector3(Input.GetAxis(Horizontal), Input.GetAxis(Vertical), 0);
		transform.position = player.transform.position + direction * 2;
	}
}
