using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	[Tooltip( "The interval (in seconds) between rounds." )]
	public float cooldown;

	[Tooltip( "The prefab for the bullet this gun will fire." )]
	public GameObject projectile;
	[Tooltip( "The prefab for the particle system that will be used to emit the shell casing for the gun." )]
	public ParticleSystem casingEmitter;
	[Tooltip( "The prefab for the particle system that will create the muzzle flash for this gun." )]
	public ParticleSystem muzzleFlash;

	[Tooltip( "If marked, the weapon will not track ammo usage and will always fire." )]
	public bool infiniteAmmo;
	[Tooltip( "The ammount of ammunition this weapon starts with. This number is the number of rounds that can be fired." )]
	public int startingAmmo;

	[Range( 0.0f, 180.0f ), Tooltip( "The angle of the gun's bullet spray." )]
	public float sprayAngle;
	[Tooltip( "If not marked, the spray will only go side-to-side, not up-and-down, so for the player the bullet spray won't cause them to shoot into the ground." )]
	public bool circleSpray;

	protected float _halfSpray;
	protected int _ammo;

	private bool _cooling;

	void Awake()
	{
		projectile.CreatePool( 100 );

		// initialize ammunition and reloading
		ammo = startingAmmo;

		_halfSpray = 0.5f * sprayAngle;
	}

	public void RefreshAmmo()
	{
		ammo = startingAmmo;
	}

	public override void PerformPrimaryAttack()
	{
		if ( canFire )
		{
			// instantiate and initialize a bullet
			InitializeBullet( projectile.Spawn() );
			PlayPrimarySound();

			// update ammunition data
			if ( !infiniteAmmo )
			{
				_ammo--;
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

			Vector3 bulletRot = bullet.transform.eulerAngles;
			bulletRot.x = 0f;
			bullet.transform.eulerAngles = bulletRot;
		}

		// make that shit go forward
		Projectile projectile = bullet.GetComponent<Projectile>();
		bullet.rigidbody.velocity = bullet.transform.forward * projectile.speed;
	}

	public void StartCooldown()
	{
		_cooling = true;
		Invoke( "EndCooldown", cooldown );
	}

	public void EndCooldown()
	{
		_cooling = false;
	}

	public void SetCooldown( float newCooldown )
	{
		cooldown = newCooldown;
	}

	public void PlayPrimarySound()
	{
		audio.Play();
	}

	public bool isOnCooldown
	{
		get
		{
			return _cooling;
		}
	}

	public bool isOutOfAmmo
	{
		get
		{
			return ammo == 0;
		}
	}

	public int ammo
	{
		get
		{
			return _ammo;
		}

		set
		{
			_ammo = ( infiniteAmmo ) ? Mathf.Max( _ammo, 1 ) : value; // this ensures there's at least 1 round available
		}
	}

	public bool canFire
	{
		get
		{
			return !isOnCooldown && ( !isOutOfAmmo || infiniteAmmo );
		}
	}
}
