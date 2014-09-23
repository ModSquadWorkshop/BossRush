using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	public GameObject projectile;

	public override void PerformPrimaryAttack()
	{
		// instantiate and initialize a bullet
		InitializeBullet( Instantiate( projectile ) as GameObject );

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