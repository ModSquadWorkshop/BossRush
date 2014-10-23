using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour
{
	public Transform lookTarget;
	public Transform playerModel;

	public float baseSpeed;
	public float speedMultiplier;

	public float lookSpeed = 10.0f;
	public float dashDistance;
	public float dashTime;

	private HealthSystem _playerHealth;
	private CameraFollow _camShake;
	private RumbleManager _rumbler;

	private Plane _plane;

	void Awake()
	{
		_plane = new Plane( Vector3.up, this.transform.position );
		_playerHealth = this.GetComponent<HealthSystem>();
		_camShake = Camera.main.gameObject.GetComponent<CameraFollow>();
		_rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();
	}

	void Start()
	{
		//register for damage callback (rumble and shake)
		_playerHealth.RegisterHealthCallback( TargetDamageCallback );
	}

	void Update()
	{
		float horizontalAxis = Input.GetAxis( "Horizontal" );
		float verticalAxis = Input.GetAxis( "Vertical" );
		// cardinal movement
		Vector3 movement = new Vector3( Input.GetAxis( "Horizontal" ),
		                                0.0f,
		                                Input.GetAxis( "Vertical" ) );
		rigidbody.velocity = movement * baseSpeed * speedMultiplier;

		//Direction the player is moving in, used for dashing
		Vector3 forwardDash = new Vector3( horizontalAxis, 0.0f, verticalAxis );

		// handle mouse input
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		float hitDistance = 0.0f;
		if ( _plane.Raycast( ray, out hitDistance ) )
		{
			lookTarget.position = ray.GetPoint( hitDistance );
		}
//		lookTarget.renderer.enabled = true;

		// handle game pad look
		// game pad look overrides mouse movement
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ),
		                                   0.0f,
		                                   Input.GetAxis( "Look Vertical" ) );
		if ( gamePadLook.sqrMagnitude > 0.0f )
		{
			lookTarget.localPosition = gamePadLook;

			// hide look target
			//lookTarget.renderer.enabled = false;
		}

		//Dash implementation using iTween
		if ( Input.GetButtonDown( "Dash" ) )
		{
			iTween.MoveTo( this.gameObject, this.gameObject.transform.position + ( forwardDash * dashDistance ) , dashTime );
			//Debug.Log( forwardDash );
		}


		// don't actually rotate the root Player object,
		// rotate the model.
		playerModel.transform.LookAt( lookTarget );
	}

	public float speed
	{
		get
		{
			return baseSpeed * speedMultiplier;
		}
	}

	void TargetDamageCallback( HealthSystem playerHealth, float damage )
	{
		_camShake.Shake( damage );
		_rumbler.rumble = true;
		_rumbler.Rumble();
	}

}
