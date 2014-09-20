using UnityEngine;
using System.Collections;

public class Gun : Weapon 
{
	public const string NAME = "gun";
	public const float COOLDOWN = 0.15f;
	public const float DAMAGE = 25.0f;

	public static Weapon Instantiate () 
	{
		return (Weapon)new Gun();
	}

	public override Weapon Clone () 
	{
		Gun weapon = new Gun();
		weapon.parent = parent;
		weapon.damage = damage;
		return (Weapon)weapon;
	}

	public override void PerformPrimaryAttack () 
	{
		if ( !_cooldownTimer.IsComplete() ) 
		{
			return;
		}

		// instantiate and initialize a bullet
		GameObject bullet = (GameObject)WeaponManager.Instantiate( Resources.Load( "bullet" ) );
		InitializeBullet( bullet );

		_cooldownTimer.Reset( true );
	}

	private void InitializeBullet ( GameObject bullet ) 
	{
		DamageSystem bulletDamage = (DamageSystem)bullet.GetComponent( typeof(DamageSystem) );
		if ( bulletDamage == null ) 
		{
			bulletDamage = (DamageSystem)bullet.AddComponent( typeof(DamageSystem) );
		}

		if ( parent != null ) 
		{
			bullet.transform.position = parent.transform.position;
			bullet.transform.rotation = parent.transform.rotation;

			DamageSystem parentDamage = (DamageSystem)parent.GetComponent( typeof(DamageSystem) );
			if ( parentDamage != null) 
			{
				// the bullet inherits the damage properties from its parent 
				// which is the object that created it (e.g. the player or perhaps an enemy)
				bulletDamage.Inherit( parentDamage );
			}
		}

		bulletDamage.destroyAfterDamage = true;
		bulletDamage.baseDamage = GetDamage();
	}

	protected override float GetDamage () 
	{
		return DAMAGE;
	}

	protected override float GetCooldownTime () 
	{
		return COOLDOWN;
	}
}