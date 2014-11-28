using UnityEngine;
using System.Collections;

public class Shotgun : Gun
{
	public int shellsPerShot = 6;


	public override void PerformPrimaryAttack()
	{
		if ( _ammo > 0 )
		{
			for ( int i = 0; i < shellsPerShot; i++ )
			{
				InitializeBullet( projectile.Spawn() );
			}
			
			casingEmitter.particleSystem.Emit( 1 );
			PlayPrimarySound();
			audio.volume = .6f;
			audio.priority = 90;

			// update ammunition data
			if ( !infiniteAmmo )
			{
				_ammo--;
			}

			StartCooldown();
		}
	}
}
