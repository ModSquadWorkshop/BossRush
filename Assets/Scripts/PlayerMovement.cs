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
	private Vector3 _dashForward;

	HealthSystem playerHealth;
	CameraFollow camShake;
	RumbleManager rumbler;

	void Start()
	{
		//register for damage callback (rumble and shake)
		playerHealth = this.GetComponent<HealthSystem>();
		playerHealth.RegisterHealthCallback( TargetDamageCallback );

		camShake = Camera.main.gameObject.GetComponent<CameraFollow>();
		rumbler = Camera.main.gameObject.GetComponent<RumbleManager>();

		_dashTimer = new Timer( dashTime, 1 );
		_dashDelayTimer = new Timer( dashDelay, 1 );
	}

	void Update()
	{
		if ( !_dashTimer.running )
		{
			float horizontalAxis = Input.GetAxis( "Horizontal" );
			float verticalAxis = Input.GetAxis( "Vertical" );

			Vector3 forward = new Vector3( horizontalAxis, 0.0f, verticalAxis );

			// cardinal movement
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

			_dashDelayTimer.Update();

			// handle dash
			if ( Input.GetButtonDown( "Dash" ) )
			{
				if ( !_dashDelayTimer.running )
				{
					_dashTimer.Reset( true );
					_dashForward = forward * dashDistance;

					if ( stopWeaponInDash )
					{
						WeaponSystem weapons = this.GetComponent<WeaponSystem>();
						weapons.currentWeapon.enabled = false;
					}
				}
			}

			// don't actually rotate the root Player object,
			// rotate the model.
			playerModel.transform.LookAt( lookTarget );
		}
		else
		{
			rigidbody.velocity = _dashForward * speed;

			_dashTimer.Update();

			if ( _dashTimer.complete )
			{
				if ( stopWeaponInDash )
				{
					WeaponSystem weapons = this.GetComponent<WeaponSystem>();
					weapons.currentWeapon.enabled = true;
				}

				_dashDelayTimer.Reset( true );
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

	void TargetDamageCallback( HealthSystem playerHealth, float damage )
	{
		camShake.Shake( damage );
		rumbler.rumble = true;
		rumbler.Rumble();
	}

}
