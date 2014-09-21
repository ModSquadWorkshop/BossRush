using UnityEngine;
using System.Collections;

public class DamageSystem : MonoBehaviour 
{
	public float baseDamage = 0.0f;
	public float damageMultiplier = 1.0f;
	public bool destroyAfterDamage = false;
	public string[] targets;

	private Hashtable _targets;

	void Start () 
	{
		// a list of targets should be added to the object prior to initilization
		InitTargets();
	}

	void OnTriggerEnter ( Collider other ) 
	{
		// check to see if the colliding object is marked as a target
		// if it is, we can proceed with dealing damage
		if ( IsTarget( other.gameObject.tag ) ) 
		{
			// check to see if the colliding object has a health system
			// if it does, we perform damage to the health
			// if it doesnt, nothing happens
			HealthSystem healthSystem = (HealthSystem)other.GetComponent( typeof(HealthSystem) );
			if ( healthSystem != null ) 
			{
				healthSystem.Damage( CalculateDamage() );
			}

			// optionally, this object can be destroyed after dealing damage
			// this is useful for objects such as bullets or suicide bombers
			if ( destroyAfterDamage ) 
			{
				Destroy( this.gameObject );
			}
		}
	}

	public float CalculateDamage () 
	{
		// as of now, damage is calculated as baseDamage * damageMultiplier
		// in the future, more complex damage calculations can be added
		// 		miss percentages, crit percentages, and other percentages can be included in the calculation, for example
		return baseDamage * damageMultiplier;
	}

	public void Inherit ( DamageSystem other ) 
	{
		baseDamage = other.baseDamage;
		damageMultiplier = other.damageMultiplier;
		targets = other.targets;

		InitTargets();
	}

	private void InitTargets () 
	{
		if ( _targets == null ) 
		{
			_targets = new Hashtable();
		} 
		else 
		{
			_targets.Clear();
		}

		// only the specified targets are affected by collision
		for ( int i = 0; i < targets.Length; i++ ) 
		{
			_targets[targets[i]] = true;
		}
	}

	private bool IsTarget ( string tag ) 
	{
		return _targets[tag] != null;
	}
}