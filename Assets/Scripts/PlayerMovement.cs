using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour
{
	public Transform lookTarget;
	public Transform playerModel;

	public float baseSpeed = 200.0f;
	public float speedMultiplier = 1.0f;

	public float lookSpeed = 10.0f;
	public float dashDistance;
	public float dashTime;

	private float _speed;

	HealthSystem playerHealth;
	CameraFollow camShake;
	RumbleManager rumbler;


	void Start()
	{
		_speed = baseSpeed * speedMultiplier;
		//register for damage callback (rumble and shake)
		playerHealth = this.GetComponent<HealthSystem>();
		playerHealth.RegisterDamageCallback( TargetDamageCallback );
		camShake = Camera.main.gameObject.GetComponent<CameraFollow>();
		rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();
	}

	void Update()
	{
		float horizontalAxis = Input.GetAxis( "Horizontal" );
		float verticalAxis = Input.GetAxis( "Vertical" );
		// cardinal movement
		Vector3 movement = new Vector3( horizontalAxis * Time.deltaTime * _speed * speedMultiplier,
		                                0.0f,
		                                verticalAxis   * Time.deltaTime * _speed * speedMultiplier );
		rigidbody.AddForce( movement * 750.0f );

		//Direction the player is moving in, used for dashing
		Vector3 forwardDash = new Vector3( horizontalAxis, 0.0f, verticalAxis );

		// handle mouse input
		lookTarget.Translate( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) ) * lookSpeed );

		// handle game pad look
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ),
		                                   0.0f,
		                                   Input.GetAxis( "Look Vertical" ) );
		if ( gamePadLook.sqrMagnitude > 0.0f )
		{
			// hide look target
			//lookTarget.renderer.enabled = false;

			lookTarget.localPosition = gamePadLook;
		}

		//Dash implementation using iTween
		if ( Input.GetButtonDown( "Dash" ) )
		{
			iTween.MoveTo( this.gameObject, this.gameObject.transform.position + (forwardDash * dashDistance) , dashTime );
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
			return _speed * speedMultiplier;
		}
		set
		{
			_speed = value;
		}
	}

	void TargetDamageCallback( HealthSystem playerHealth, float damage )
	{
		camShake.Shake( damage );
		rumbler.rumble = true;
		rumbler.Rumble();
	}
	/*
	void OnCollisionEnter( Collision other )
	{
		if ( other.gameObject.tag == "Scenery" )
		{
			iTween.Stop( this.gameObject );
		}
	}
	 */

}
