using UnityEngine;
using System.Collections;
using System.Collections.Generic;

sealed public class DamageSystem : MonoBehaviour
{
	public float baseDamage = 0.0f;
	public float damageMultiplier = 1.0f;
	[Tooltip( "If marked the damage will be multiplied by frame time, meaning the damage amount will be damage-per-second." )]
	public bool damageOverTime;
	[Tooltip( "If marked the object will destroy itself after dealing damage. This is useful for bullet, bombs, and other one-time use weapons." )]
	public bool destroyAfterDamage = false;
	[Tooltip( "A list of the tags that mark a valid target." )]
	public List<string> targets;

	private Hashtable _targets;

	void Awake()
	{
		// a list of targets should be added to the object prior to initilization
		InitTargets();
	}

	void OnCollisionEnter( Collision collision )
	{
		// check to see if the colliding object is marked as a target
		// if it is, we can proceed with dealing damage
		if ( IsTarget( collision.gameObject.tag ) )
		{
			// try to damage the colliding object
			DamageObject( collision.gameObject );

			// optionally, this object can be destroyed after dealing damage
			// this is useful for objects such as bullets or suicide bombers
			if ( destroyAfterDamage )
			{
				// if we're on a projectile, explode that projectile
				Projectile projectile = this.gameObject.GetComponent<Projectile>();
				if ( projectile != null )
				{
					projectile.Explode( collision );
				}

				GetComponent<DeathSystem>().Kill();
			}
		}
	}

	public void DamageObject( GameObject target )
	{
		// check to see if the colliding object has a health system
		// if it does, we perform damage to the health
		// if it doesnt, nothing happens
		// check to see if the colliding object has a health system
		HealthSystem healthSystem = target.GetComponent<HealthSystem>();
		if ( healthSystem != null )
		{
			healthSystem.Damage( CalculateDamage() );
		}
	}

	public void DamageObjectIfTarget( GameObject target )
	{
		if ( IsTarget( target.tag ) )
		{
			DamageObject( target );
		}
	}

	public float CalculateDamage()
	{
		if ( damageOverTime )
		{
			return baseDamage * damageMultiplier * Time.deltaTime;
		}
		else
		{
			return baseDamage * damageMultiplier;
		}
	}

	public void Inherit( DamageSystem other )
	{
		baseDamage = other.baseDamage;
		damageMultiplier = other.damageMultiplier;
		targets = other.targets;

		InitTargets();
	}

	public bool IsTarget( string tag )
	{
		return _targets[tag] != null;
	}

	private void InitTargets()
	{
		if ( _targets == null )
		{
			_targets = new Hashtable();
		}
		else
		{
			_targets.Clear();
		}

		if ( targets != null )
		{
			// only the specified targets are affected by collision
			for ( int i = 0; i < targets.Count; i++ )
			{
				_targets[targets[i]] = true;
			}
		}
	}
}
