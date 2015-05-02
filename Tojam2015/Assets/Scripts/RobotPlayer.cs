using UnityEngine;
using System.Collections;

public class RobotPlayer : MonoBehaviour {
    
    public float jumpForce;
    public float surfaceWalkSpeed;
    public string horizontalAxis;
    public string jumpButton;
    public PlayerState playerState;

    Animator _animator;
    ForceField _forceField;

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

    void Start()
    {
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();
    }
	
	// Update is called once per frame
	void Update () {

        if (_state == PlayerState.Floating)
        {
            // Sample gravity from field
            _cf2D.force = _forceField.GetForce(transform.position);            
            _animator.SetBool("Floating", true);            
        }
        else if (_state == PlayerState.AboutToLand)
        {
            // Turn off physics and fix to surface
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
            float hInput = Input.GetAxis(horizontalAxis);
            Vector2 surfacePosition = transform.position - _surface.transform.position;

            if (Mathf.Abs(hInput) > 0)
            {                
                // Move (rotate) around planet surface
                float theta = Mathf.Atan2(surfacePosition.y, surfacePosition.x);
                theta -= hInput * surfaceWalkSpeed * Time.deltaTime / _surface.radius;

                transform.position = _surface.transform.position + _surface.radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);                

                _animator.SetBool("Walking", true);
            }
            else
            {
                _animator.SetBool("Walking", false);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfacePosition.normalized);

            if (Input.GetButton(jumpButton))
            {                
                // Give a nudge off the surface before turning physics back on
                transform.position = _surface.transform.position + (Vector3) surfacePosition * 1.01f;
                _animator.SetBool("Walking", false);
                _rb2D.isKinematic = false;
                _rb2D.AddForce(surfacePosition.normalized * jumpForce, ForceMode2D.Impulse);                
                _state = playerState = PlayerState.LeavingSurface;           
            }
        }
        else if (_state == PlayerState.LeavingSurface)
        {
            // Placeholder
            _state = playerState = PlayerState.Floating;            
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
