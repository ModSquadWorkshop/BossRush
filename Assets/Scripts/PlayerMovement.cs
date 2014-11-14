using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour
{
	public Transform lookTarget;
	public Transform playerModel;
	public PlayerCrosshairs crosshairs;

	public float baseSpeed;
	public float speedMultiplier;

	public float lookSpeed = 10.0f;

	public float dashDistance;
	public float dashSpeed;
	public float dashDelay;
	public bool  stopWeaponInDash;

	private Vector3 _forwardVect;

	private RaycastHit	_dashHit;
	private Vector3		_dashOrigin;
	private Vector3		_dashVelocity;
	private float		_dashMaxDistance;
	private int			_dashLayerMask;
	private bool		_dashing;
	private bool		_dashAvailable;

	private WeaponSystem	_playerWeapons;
	private HealthSystem	_playerHealth;
	private CameraShake		_camShake;
	private RumbleManager	_rumbler;

	private Plane _plane;

	void Awake()
	{
		_playerHealth = GetComponent<HealthSystem>();
		_playerWeapons = GetComponent<WeaponSystem>();
		_plane = new Plane( Vector3.up, this.transform.position );

		// create a ray casting layer mask that collides with everything accept "Player"
		_dashLayerMask = 1 << LayerMask.NameToLayer( "Player" );
		_dashLayerMask = ~_dashLayerMask; // invert the mask
		_dashAvailable = true;
	}

	void Start()
	{
		// register for damage callback (rumble and shake)
		_playerHealth.RegisterHealthCallback( TargetDamageCallback );

		_camShake = Camera.main.gameObject.GetComponent<CameraShake>();
		_rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();

		_forwardVect = new Vector3();
	}

	void Update()
	{
		if ( !_dashing )
		{
			/* update the player while not dashing */

			// get the input forward vector
			_forwardVect.Set( Input.GetAxis( "Horizontal" ), 0.0f, 
							  Input.GetAxis( "Vertical" ) );

			// apply general movement
			rigidbody.velocity = _forwardVect * speed;

			// handle look direction
			HandleLookDirection();

			// handle dash input
			if ( Input.GetButtonDown( "Dash" ) )
			{
				Dash();
			}
		}
		else
		{
			/* update the player while dashing */
			DashUpdate();
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
		// handle mouse input
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		float hitDistance = 0.0f;
		if ( _plane.Raycast( ray, out hitDistance ) )
		{
			lookTarget.position = ray.GetPoint( hitDistance );
		}

		bool mouseMoved = ( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, 
										 Input.GetAxis( "Mouse Y" ) ) ).sqrMagnitude > 0.0f;

		// handle game pad look
		// game pad look overrides mouse movement
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f,
		                                   Input.GetAxis( "Look Vertical" ) );

		bool controllerMoved = gamePadLook.sqrMagnitude > 0.0f;
		if ( controllerMoved )
		{
			lookTarget.localPosition = gamePadLook;
			crosshairs.show = false;
		}

		if ( mouseMoved )
		{
			crosshairs.show = true;
		}

		// don't actually rotate the root Player object, rotate the model
		playerModel.transform.LookAt( lookTarget );
	}

	private void Dash()
	{
		if ( _dashAvailable )
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
			_dashVelocity = _forwardVect * dashSpeed;
			_dashAvailable = false;
			_dashing = true;

			Invoke( "DashComplete", dashDistance / dashSpeed );
		}
	}

	private void DashUpdate()
	{
		rigidbody.velocity = _dashVelocity;

		if ( Vector3.Distance( _dashOrigin, rigidbody.transform.position ) >= _dashMaxDistance )
		{
			/* a partial dash occured because of collision */

			CancelInvoke( "DashComplete" );
			DashComplete();

			rigidbody.transform.position = _dashHit.point;
			rigidbody.velocity = Vector3.zero;
		}
	}

	private void DashComplete()
	{
		_playerWeapons.currentWeapon.enabled = true;
		_dashing = false;

		Invoke( "DashDelayComplete", dashDelay );
	}

	private void DashDelayComplete()
	{
		_dashAvailable = true;
	}

	private void TargetDamageCallback( HealthSystem playerHealth, float healthChange )
	{
		if ( healthChange < 0.0f )
		{
			_camShake.Shake( healthChange );
			_rumbler.Rumble( healthChange );
		}
	}
}
