using UnityEngine;
using System.Collections;

public class WallTeleporter : MonoBehaviour {

	public GameObject oppositeWall;
	public bool teleportX; //Determine whether we teleport horizontally or vertically.
	public static float offset = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
		}
		Vector2 otherPosition = other.transform.position;
		if (teleportX){
			if (gameObject.transform.position.x < oppositeWall.gameObject.transform.position.x){ //This wall is on the left
				otherPosition.x = oppositeWall.transform.position.x - offset;
			}
			else {otherPosition.x = oppositeWall.transform.position.x + offset;}
		}
		else
		{
			if (gameObject.transform.position.y < oppositeWall.gameObject.transform.position.y){ //This wall is below the other wall
				otherPosition.y = oppositeWall.transform.position.y - offset;
			}
			else{
				otherPosition.y = oppositeWall.transform.position.y + offset;
			}
		}

		other.transform.position = otherPosition;
	}
}
