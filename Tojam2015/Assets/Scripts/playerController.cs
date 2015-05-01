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
			RaycastHit2D hit = Physics2D.Raycast(transform.position,  Camera.main.ScreenToWorldPoint (Input.mousePosition));
			Vector3 target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (hit.point != null){
				target.z = 0;
				Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);
				Instantiate(projectileObject, transform.position * 0, projectileRotation);
			}
		}
	}
}
