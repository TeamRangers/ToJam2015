using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {
	public GameObject projectileObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


			

		if (Input.GetButtonDown("Fire1")){
			Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			target.z = 0;
			Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);
			Instantiate(projectileObject, Vector3.MoveTowards(transform.position, target, 1), projectileRotation);
		}
	}
}
