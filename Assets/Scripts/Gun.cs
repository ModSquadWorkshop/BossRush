using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	public GameObject projectile;
	public Transform casingAnchor;
	public GameObject casingEmitter;

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
	}

	public override void PerformPrimaryAttack()
	{
		// instantiate and initialize a bullet
		InitializeBullet( Instantiate( projectile ) as GameObject );

		// create shell casing
		casingEmitter.particleSystem.Emit( 1 );

		// reset and start the gun cooldown
		_cooldownTimer.Reset( true );
	}

	private void InitializeBullet( GameObject bullet )
	{
		DamageSystem bulletDamage = bullet.GetComponent( typeof( DamageSystem ) ) as DamageSystem;
		if ( bulletDamage == null )
		{
			bulletDamage = bullet.AddComponent( typeof( DamageSystem ) ) as DamageSystem;
		}

		DamageSystem weaponDamage = GetComponent( typeof( DamageSystem ) ) as DamageSystem;
		if ( weaponDamage != null )
		{
			// the bullet inherits the damage properties from the gun that fired it
			bulletDamage.Inherit( weaponDamage );
		}

		bulletDamage.destroyAfterDamage = true;

		// set the bullet to spawn as a child of the gun
		bullet.transform.position = transform.position;
		bullet.transform.rotation = transform.rotation;
	}
}