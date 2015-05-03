using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ConstantForce2D))]
public class ForceFieldBody : MonoBehaviour {

    ConstantForce2D _cf2D;
    ForceField _forceField;

	// Use this for initialization
	void Start () {
        _cf2D = GetComponent<ConstantForce2D>();
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        _cf2D.force = _forceField.GetForce(transform.position);
	}
}
