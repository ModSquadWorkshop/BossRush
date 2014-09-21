using UnityEngine;
using System.Collections;

public class Gun : Weapon 
{
	public GameObject projectile;

	public override void PerformPrimaryAttack() 
	{
		// instantiate and initialize a bullet
		InitializeBullet( (GameObject)Instantiate( projectile) );

		// reset and start the gun cooldown
		_cooldownTimer.Reset( true );
	}

	private void InitializeBullet( GameObject bullet ) 
	{
		DamageSystem bulletDamage = (DamageSystem)bullet.GetComponent( typeof( DamageSystem ) );
		if ( bulletDamage == null ) 
		{
			bulletDamage = (DamageSystem)bullet.AddComponent( typeof( DamageSystem ) );
		}

		DamageSystem weaponDamage = (DamageSystem)GetComponent( typeof( DamageSystem ) );
		if ( weaponDamage != null) 
		{
			// the bullet inherits the damage properties from the gun that fired it 
			bulletDamage.Inherit( weaponDamage );
		}

		bulletDamage.destroyAfterDamage = true;

		// set the bullet to spawn as a child of the gun
		bullet.transform.position = this.transform.position;
		bullet.transform.rotation = this.transform.rotation;
	}
}