using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	public GameObject projectile;

	public Transform casingAnchor;
	public GameObject casingEmitter;

	public bool infiniteAmmo;
	public int ammoPerMagazine;
	public int amountOfMagazines;
	public float reloadSpeed;

    public float shake;

	[Range( 0.0f, 180.0f )]
	public float sprayAngle;
	public bool circleSpray;

	private float _halfSpray;

	[SerializeField] private int _magazines;
	[SerializeField] private int _magazineAmmo;

	private bool _reloading;
	private Timer _reloadTimer;

	public override void Start()
	{
		// we still need to call Weapon.Start() to initialize the timer and whatnot.
		base.Start();

		// instantiate casing emitter
		casingEmitter = Instantiate( casingEmitter ) as GameObject;
		casingEmitter.transform.parent = casingAnchor;

		// we need to reset the transform so that it adhere's to the anchor's transform,
		// otherwise unity try's to preserve it's exising location in world space
		// when you child an object to another object.
		casingEmitter.transform.localPosition = Vector3.zero;
		casingEmitter.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, 0.0f );

		// initialize ammunition and reloading
		_magazines = amountOfMagazines;
		_magazineAmmo = ( _magazines > 0 ) ? ammoPerMagazine : 0;
		_magazineAmmo = ( infiniteAmmo ) ? Mathf.Max( _magazineAmmo, 1 ) : _magazineAmmo; // this line just insures you have atleast 1 ammo available
		_reloading = false;
		_reloadTimer = new Timer( reloadSpeed, 1 );

		_halfSpray = 0.5f * sprayAngle;
	}

	public override void Update()
	{
		if ( _reloading )
		{
			_reloadTimer.Update();

			if ( _reloadTimer.IsComplete() )
			{
				// stop reloading
				_reloading = false;

				// update ammo
				_magazines--;
				_magazineAmmo = ammoPerMagazine;
			}
		}

		base.Update();
	}

	public override void PerformPrimaryAttack()
	{
		if ( _reloading || _magazineAmmo <= 0 )
		{
			return;
		}

		// instantiate and initialize a bullet
		InitializeBullet( Instantiate( projectile ) as GameObject );

		// update ammunition data
		if ( !infiniteAmmo )
		{
			_magazineAmmo--;

			if ( _magazineAmmo <= 0 )
			{
				Reload();
			}
		}

		// create shell casing
		casingEmitter.particleSystem.Emit( 1 );

		// reset and start the gun cooldown
		_cooldownTimer.Reset( true );
	}

	public void Reload()
	{
		// if already reloading, don't reload again
		// and, reloading is only possible if another ammo clip is available
		if ( !_reloading && _magazines > 0 )
		{
			// start reloading
			_reloading = true;
			_reloadTimer.Reset( true );
		}
	}

	public bool IsOutOfAmmo()
	{
		return GetTotalAmmo() == 0;
	}

	public int GetTotalAmmo()
	{
		return ( _magazines * ammoPerMagazine ) + _magazineAmmo;
	}

	public bool reloading
	{
		get
		{
			return _reloading;
		}
	}

	private void InitializeBullet( GameObject bullet )
	{
		DamageSystem bulletDamage = bullet.GetComponent<DamageSystem>();
		if ( bulletDamage == null )
		{
			bulletDamage = bullet.AddComponent<DamageSystem>();
		}

		DamageSystem weaponDamage = GetComponent<DamageSystem>();
		if ( weaponDamage != null )
		{
			// the bullet inherits the damage properties from the gun that fired it
			bulletDamage.Inherit( weaponDamage );
		}

        bulletDamage.shakeForce = shake;
		bulletDamage.destroyAfterDamage = true;

		// set the bullet to spawn as a child of the gun
		bullet.transform.position = transform.position;
		if ( circleSpray )
		{
			// create circular spray by rotating by _halfSpray to the side and then spinning it a random amount.
			bullet.transform.rotation = transform.rotation * Quaternion.Euler( Random.Range( 0.0f, _halfSpray ), 0.0f, Random.Range( 0.0f, 360.0f ) );
		}
		else
		{
			// flat spray (good for the player, since a circle spray makes them shoot the ground).
			// pick a random rotation between -_halfSpray and _halfSpray.
			bullet.transform.rotation = transform.rotation * Quaternion.Euler( 0.0f, Random.Range( -_halfSpray, _halfSpray ), 0.0f );
		}

        
	}
}
