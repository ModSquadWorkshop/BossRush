using UnityEngine;
using System.Collections;

public class Shotgun : Gun
{
	public int shellsPerShot = 6;


	public override void PerformPrimaryAttack()
	{
		if ( !_reloading && _magazineAmmo > 0 )
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
				_magazineAmmo--;

				if ( _magazineAmmo <= 0 )
				{
					Reload();
				}
			}

			StartCooldown();
		}
	}
}
