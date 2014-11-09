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
	public float dashSpeed;
	public float dashDelay;
	public bool stopWeaponInDash;

	private Vector3 _forwardVect;
	private Vector3 _lookTargetVect;
	private Vector3 _gamPadVect;

	private Timer _dashTimer;
	private Timer _dashDelayTimer;
	private Vector3 _dashOrigin;
	private Vector3 _dashVelocity;
	private RaycastHit _dashHit;
	private float _dashMaxDistance;
	private float _dashDistanceTraveled;
	private int _dashLayerMask;

	private WeaponSystem _playerWeapons;

	private HealthSystem playerHealth;
	private CameraShake camShake;
	private RumbleManager rumbler;

	void Start()
	{
		// register for damage callback (rumble and shake)
		playerHealth = this.GetComponent<HealthSystem>();
		playerHealth.RegisterHealthCallback( TargetDamageCallback );

		camShake = Camera.main.gameObject.GetComponent<CameraShake>();
		rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();

		_forwardVect = new Vector3();
		_lookTargetVect = new Vector3();
		_gamPadVect = new Vector3();

		// initialize dash timers
		_dashTimer = new Timer( dashDistance / dashSpeed, 1 );
		_dashDelayTimer = new Timer( dashDelay, 1 );

		// create a ray casting layer mask that collides with everything accept "Player"
		_dashLayerMask = 1 << LayerMask.NameToLayer( "Player" );
		_dashLayerMask = ~_dashLayerMask; // invert the mask

		_playerWeapons = this.GetComponent<WeaponSystem>();
	}

	void Update()
	{
		if ( !_dashTimer.running )
		{
			/* update the player while not dashing */

			// get the input forward vector
			float horizontalAxis = Input.GetAxis( "Horizontal" );
			float verticalAxis = Input.GetAxis( "Vertical" );
			_forwardVect.Set( horizontalAxis, 0.0f, verticalAxis );

			// apply general movement
			rigidbody.velocity = _forwardVect * speed;

			// handle look direction
			HandleLookDirection();

			// update the dash delay timer
			// the player won't be able to dash again until this timer is done running
			_dashDelayTimer.Update();

			// handle dash input
			if ( Input.GetButtonDown( "Dash" ) )
			{
				Dash();
			}
		}
		else
		{
			/* update the player while dashing */

			DashingUpdate();
		}
	}

	public float speed
	{
		get
		{
			return baseSpeed * speedMultiplier;
		}
	}

	private void HandleLookDirection()
	{
		// handle mouse input look
		_lookTargetVect.Set( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) );
		lookTarget.Translate( _lookTargetVect * lookSpeed );

		// handle game pad look
		_gamPadVect.Set( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		if ( _gamPadVect.sqrMagnitude > 0.0f )
		{
			lookTarget.localPosition = _gamPadVect;
		}

		// don't actually rotate the root Player object, rotate the model
		playerModel.transform.LookAt( lookTarget );
	}

	private void Dash()
	{
		// insure the player isn't already dashing
		if ( !_dashDelayTimer.running )
		{
			_dashOrigin = rigidbody.transform.position;

			// calculate if the dash distance needs to be shorter according to any collisions that will happen
			if ( Physics.Raycast( _dashOrigin, _forwardVect, out _dashHit, Mathf.Infinity, _dashLayerMask ) )
			{
				// the dash distance is limited to the closest colliding object
				_dashMaxDistance = _dashHit.distance;
			}
			else
			{
				// the dash distance is not limited because no collisions will occur
				_dashMaxDistance = Mathf.Infinity;
			}

			if ( stopWeaponInDash )
			{
				// disable the player's weapon
				_playerWeapons.currentWeapon.enabled = false;
			}

			// start the dash
			_dashDistanceTraveled = 0.0f;
			_dashVelocity = _forwardVect * dashSpeed;
			_dashTimer.Reset( true );
		}
	}

	private void DashingUpdate()
	{
		// update dash properties to determine if it needs to be stopped
		_dashDistanceTraveled = Vector3.Distance( _dashOrigin, rigidbody.transform.position );
		_dashTimer.Update();

		if ( _dashTimer.complete )
		{
			/* a full dash occured */

			StopDash();
		}
		else if ( _dashDistanceTraveled >= _dashMaxDistance )
		{
			/* a partial dash occured because of collision */

			StopDash();

			rigidbody.transform.position = _dashHit.point;
			rigidbody.velocity = Vector3.zero;
		}
		else
		{
			/* still currently dashing */

			rigidbody.velocity = _dashVelocity;
		}
	}

	private void StopDash()
	{
		// re-enable the player's weapon
		_playerWeapons.currentWeapon.enabled = true;

		// start the dash delay timer
		// the player won't be able to dash again until this timer is complete
		_dashDelayTimer.Reset( true );
	}

	private void TargetDamageCallback( HealthSystem playerHealth, float damage )
	{
		//Debug.Log( damage );
		if ( damage < 0 )
		{
			rumbler.Rumble( damage );
			camShake.Shake( damage );
		}
	}
}
