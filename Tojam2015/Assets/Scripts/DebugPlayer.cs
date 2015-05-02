using UnityEngine;
using System.Collections;

public class DebugPlayer : MonoBehaviour {

    public ForceField forceField;
    public float jumpForce;
    public float rotationSpeed;
    public PlayerState playerState;

    Animator _animator;

    public enum PlayerState
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
	void Awake () {
        _rb2D = GetComponent<Rigidbody2D>();
        _cf2D = GetComponent<ConstantForce2D>();
        _state = PlayerState.Floating;
        _surface = null;
        _animator = GetComponent<Animator>();        
	}
	
	// Update is called once per frame
	void Update () {

        if (_state == PlayerState.Floating)
        {
            _cf2D.force = forceField.GetForce(transform.position);            
            _animator.SetBool("Floating", true);            
        }
        else if (_state == PlayerState.AboutToLand)
        {
            _rb2D.isKinematic = true;
            _animator.SetBool("Floating", false);
            _cf2D.force = Vector2.zero;

            Vector2 surfaceNormal = transform.position - _surface.transform.position;            
            transform.position = (Vector3) surfaceNormal.normalized * _surface.radius + _surface.transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

            _state = playerState = PlayerState.OnSurface;
        }
        else if (_state == PlayerState.OnSurface)
        {
            float hInput = Input.GetAxis("P1_Horizontal");
            Vector2 surfacePosition = transform.position - _surface.transform.position;

            if (Mathf.Abs(hInput) > 0)
            {                
                float theta = Mathf.Atan2(surfacePosition.y, surfacePosition.x);
                theta -= hInput * rotationSpeed * Time.deltaTime;

                transform.position = _surface.transform.position + _surface.radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);                

                _animator.SetBool("Walking", true);
            }
            else
            {
                _animator.SetBool("Walking", false);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfacePosition.normalized);

            if (Input.GetButton("Jump"))
            {                
                transform.position = _surface.transform.position + (Vector3) surfacePosition * 1.1f;
                _animator.SetBool("Walking", false);
                _rb2D.isKinematic = false;
                _rb2D.AddForce(surfacePosition.normalized * jumpForce, ForceMode2D.Impulse);                
                _state = playerState = PlayerState.LeavingSurface;           
            }
        }
        else if (_state == PlayerState.LeavingSurface)
        {
            _state = playerState = PlayerState.Floating;
            Debug.Log("Current state is LeavingSurface");
        }

	}

    void OnCollisionEnter2D(Collision2D col2D)
    {
        if (col2D.gameObject.tag == "SphereAttractor" && _state == PlayerState.Floating)
        {
            _state = playerState = PlayerState.AboutToLand;
            _surface = col2D.gameObject.GetComponent<SphereAttractor>();
        }
    }
}
