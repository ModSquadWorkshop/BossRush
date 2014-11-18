using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	public GameObject projectile;

	public ParticleSystem casingEmitter;
	public ParticleSystem muzzleFlash;

	public bool infiniteAmmo;
	public int ammoPerMagazine;
	public int amountOfMagazines;
	public float reloadSpeed;

	[Range( 0.0f, 180.0f )]
	public float sprayAngle;
	public bool circleSpray;

	protected float _halfSpray;

	protected int _magazines;
	protected int _magazineAmmo;

	protected bool _reloading;

	void Awake()
	{
		// initialize ammunition and reloading
		_magazines = amountOfMagazines;
		_magazineAmmo = ammoPerMagazine; 
		_magazineAmmo = ( infiniteAmmo ) ? Mathf.Max( _magazineAmmo, 1 ) : _magazineAmmo; // this line just insures you have atleast 1 ammo available
		_reloading = false;

		_halfSpray = 0.5f * sprayAngle;
	}

	public void RefreshAmmo()
	{
		_magazines = amountOfMagazines;
		_magazineAmmo = ammoPerMagazine;
		_magazineAmmo = ( infiniteAmmo ) ? Mathf.Max( _magazineAmmo, 1 ) : _magazineAmmo; // this line just insures you have atleast 1 ammo available
		_reloading = false;
	}

	public override void PerformPrimaryAttack()
	{
		if ( !_reloading && _magazineAmmo > 0 )
		{
			// instantiate and initialize a bullet
			InitializeBullet( Instantiate( projectile ) as GameObject );
			PlayPrimarySound();

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
			casingEmitter.Emit( 1 );
			muzzleFlash.Emit( 10 );

			StartCooldown();
		}
	}

	protected void InitializeBullet( GameObject bullet )
	{
		DamageSystem bulletDamage = bullet.GetComponent<DamageSystem>();
		if ( bulletDamage == null )
		{
			bulletDamage = bullet.AddComponent<DamageSystem>();
		}

		DamageSystem weaponDamage = this.GetComponent<DamageSystem>();
		if ( weaponDamage != null )
		{
			// the bullet inherits the damage properties from the gun that fired it
			bulletDamage.Inherit( weaponDamage );
		}

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

	public void Reload()
	{
		// if already reloading, don't reload again
		// and, reloading is only possible if another ammo clip is available
		if ( !_reloading && _magazines > 0 )
		{
			// start reloading
			_reloading = true;
			Invoke( "ReloadComplete", reloadSpeed );
		}
	}

	private void ReloadComplete()
	{
		// stop reloading
		_reloading = false;

		// update ammo
		_magazines--;
		_magazineAmmo = ammoPerMagazine;
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
}
