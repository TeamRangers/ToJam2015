using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public GameObject forceField;

	// Use this for initialization
	void Start () {
        forceField.GetComponent<ForceField>().Generate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
