using UnityEngine;
using System.Collections;

public class DebugPlayer : MonoBehaviour {

    public ForceField forceField;
    public float jumpForce;
    public float rotationSpeed;

    enum PlayerState
    {
        Floating,
        AboutToLand,
        OnSurface,
        LeavingSurface,
        Dead
    };

    PlayerState _state;
    SphereAttractor _surface;

    Rigidbody2D _rb2D;
    ConstantForce2D _cf2D;

	// Use this for initialization
	void Start () {
        _rb2D = GetComponent<Rigidbody2D>();
        _cf2D = GetComponent<ConstantForce2D>();
        _state = PlayerState.Floating;
        _surface = null;
	}
	
	// Update is called once per frame
	void Update () {

        if (_state == PlayerState.Floating)
        {
            _cf2D.force = forceField.GetForce(transform.position);
        }
        else if (_state == PlayerState.AboutToLand)
        {
            _rb2D.isKinematic = true;                        

            Vector2 surfaceNormal = transform.position - _surface.transform.position;            
            transform.position = (Vector3) surfaceNormal.normalized * _surface.radius + _surface.transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

            _state = PlayerState.OnSurface;
        }
        else if (_state == PlayerState.OnSurface)
        {
            float hInput = Input.GetAxis("P1_Horizontal");

            if (Mathf.Abs(hInput) > 0)
            {
                Vector2 surfacePosition = transform.position - _surface.transform.position;

                float theta = Mathf.Atan2(surfacePosition.y, surfacePosition.x);
                theta -= hInput * rotationSpeed * Time.deltaTime;

                transform.position = _surface.transform.position + _surface.radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
                transform.rotation = Quaternion.FromToRotation(Vector3.up, surfacePosition.normalized);
            }

            if (Input.GetButton("Jump"))
            {
                Vector2 surfacePosition = transform.position - _surface.transform.position;
                transform.position = _surface.transform.position + (Vector3) surfacePosition * 1.01f;
                _rb2D.isKinematic = false;
                _rb2D.AddForce(surfacePosition.normalized * jumpForce, ForceMode2D.Impulse);
                _state = PlayerState.Floating;
            }
        }

	}

    void OnCollisionEnter2D(Collision2D col2D)
    {
        if (col2D.gameObject.tag == "SphereAttractor" && _state == PlayerState.Floating)
        {
            _state = PlayerState.AboutToLand;
            _surface = col2D.gameObject.GetComponent<SphereAttractor>();
        }
    }
}
