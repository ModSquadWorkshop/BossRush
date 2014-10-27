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
	public float dashDelay;
	public bool stopWeaponInDash;

	private Timer _dashTimer;
	private Timer _dashDelayTimer;
	private Vector3 _dashOrigin;
	private Vector3 _dashForward;
	private RaycastHit _dashHit;
	private float _dashMaxDistance;
	private float _dashDistanceTraveled;
	private int _dashLayerMask;

	HealthSystem playerHealth;
	CameraFollow camShake;
	RumbleManager rumbler;

	void Start()
	{
		// register for damage callback (rumble and shake)
		playerHealth = this.GetComponent<HealthSystem>();
		playerHealth.RegisterHealthCallback( TargetDamageCallback );

		camShake = Camera.main.gameObject.GetComponent<CameraFollow>();
		rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();

		// initialize dash timers
		_dashTimer = new Timer( dashTime, 1 );
		_dashDelayTimer = new Timer( dashDelay, 1 );

		// create a ray casting layer mask that collides with everything accept "Player"
		_dashLayerMask = 1 << LayerMask.NameToLayer( "Player" );
		_dashLayerMask = ~_dashLayerMask; // invert the mask
	}

	void Update()
	{
		if ( !_dashTimer.running )
		{
			float horizontalAxis = Input.GetAxis( "Horizontal" );
			float verticalAxis = Input.GetAxis( "Vertical" );

			Vector3 forward = new Vector3( horizontalAxis, 0.0f, verticalAxis );

			// cardinal movement (WASD)
			rigidbody.velocity = forward * speed;

			// handle mouse input
			lookTarget.Translate( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) ) * lookSpeed );

			// handle game pad look
			Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f,
											   Input.GetAxis( "Look Vertical" ) );
			if ( gamePadLook.sqrMagnitude > 0.0f )
			{
				// hide look target
				//lookTarget.renderer.enabled = false;
				lookTarget.localPosition = gamePadLook;
			}

			// update the dash delay timer
			// the player won't be able to dash again until this timer is done running
			_dashDelayTimer.Update();

			// handle dash input
			if ( Input.GetButtonDown( "Dash" ) ) 
			{
				// insure the player isn't already dashing
				if ( !_dashDelayTimer.running ) 
				{
					_dashOrigin = rigidbody.transform.position;

					// calculate if the dash distance needs to be shorter according to any collisions that will happen
					if ( Physics.Raycast( _dashOrigin, forward, out _dashHit, Mathf.Infinity, _dashLayerMask ) )
					{
						_dashMaxDistance = _dashHit.distance;
					}
					else
					{
						_dashMaxDistance = Mathf.Infinity;
					}

					if ( stopWeaponInDash ) 
					{
						// disable the player's weapon
						WeaponSystem weapons = this.GetComponent<WeaponSystem>();
						weapons.currentWeapon.enabled = false;
					}

					// start the dash
					_dashDistanceTraveled = 0.0f;
					_dashForward = forward * dashDistance;
					_dashTimer.Reset( true );
				}
			}

			// don't actually rotate the root Player object, rotate the model
			playerModel.transform.LookAt( lookTarget );
		}
		else
		{
			// update dash properties to determine if it needs to be stopped
			_dashDistanceTraveled = Vector3.Distance( _dashOrigin, rigidbody.transform.position );
			_dashTimer.Update();

			if ( _dashTimer.complete ) 
			{
				// a full dash occured

				StopDash();
			}
			else if ( _dashDistanceTraveled >= _dashMaxDistance )
			{
				// a partial dash occured because of collision

				StopDash();

				rigidbody.transform.position = _dashHit.point;
				rigidbody.velocity = Vector3.zero;
			}
			else
			{
				// still currently dashing

				rigidbody.velocity = _dashForward * speed;
			}
		}
	}

	public float speed
	{
		get
		{
			return baseSpeed * speedMultiplier;
		}
	}

	private void StopDash()
	{
		if ( stopWeaponInDash )
		{
			// enable the player's weapon
			WeaponSystem weapons = this.GetComponent<WeaponSystem>();
			weapons.currentWeapon.enabled = true;
		}

		// start the dash delay timer
		// the player won't be able to dash again until this timer is complete
		_dashDelayTimer.Reset( true );
	}

	void TargetDamageCallback( HealthSystem playerHealth, float damage )
	{
		camShake.Shake( damage );
		rumbler.rumble = true;
		rumbler.Rumble();
	}

}
