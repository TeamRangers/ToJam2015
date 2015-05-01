using UnityEngine;
using System.Collections;

public class DebugPlayer : MonoBehaviour {

    public ForceField _forceField;

    Rigidbody2D _rb2D;
    ConstantForce2D _cf2D;

	// Use this for initialization
	void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
        _cf2D = GetComponent<ConstantForce2D>();
	}
	
	// Update is called once per frame
	void Update () {
        _cf2D.force = _forceField.GetForce(transform.position);
	}
}
