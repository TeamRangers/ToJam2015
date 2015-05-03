using UnityEngine;
using System.Collections;

public class ReticleController : MonoBehaviour {

	public Transform playerTransform;
	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 playerProjectileOrigin = playerTransform.position + playerTransform.up * 0.6f;
		Vector3 maxAllowed = mousePos - playerProjectileOrigin;
		maxAllowed = Vector3.ClampMagnitude(maxAllowed, 3);
		transform.position =  playerProjectileOrigin + maxAllowed;
	}
}
