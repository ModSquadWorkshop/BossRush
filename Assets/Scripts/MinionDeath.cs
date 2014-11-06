using UnityEngine;
using System.Collections;

public class MinionDeath : MonoBehaviour
{
	public float deathTime;

	public bool exploding;
	public bool dropping;

	HealthSystem otherHealth;
	DeathTimer timedDeath;

	public GameObject dropItem;
	public GameObject explosionEffect;

	[Range( 0.0f, 1.0f )]
	public float dropChance;

	public float explosionRadius;
	public float explosionDamage;
	public float explosionForce;

	// Use this for initialization
	void Start ()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( DeathCallback );
		if ( deathTime > 0 )
		{
			timedDeath = gameObject.AddComponent<DeathTimer>();
			timedDeath.deathTime = this.deathTime;
		}
	}

	void DeathCallback( GameObject minion )
	{
		// ensure that we don't get notified of our death multiple times
		GetComponent<DeathSystem>().DeregisterDeathCallback( DeathCallback );

		if( exploding )
		{
			Explode();
		}
		if ( dropping )
		{
			Drop();
		}
	}

	void Explode()
	{
		Collider[] collisions = Physics.OverlapSphere( transform.position, explosionRadius );
		foreach ( Collider col in collisions )
		{
			otherHealth = col.gameObject.GetComponent<HealthSystem>();
			if( otherHealth != null )
			{
				otherHealth.Damage( explosionDamage );
			}
			if ( col.rigidbody != null )
			{
				col.rigidbody.AddExplosionForce( explosionForce, transform.position, explosionRadius );
			}
		}

		( Instantiate( explosionEffect, transform.position, transform.rotation ) as GameObject ).GetComponent<Explosion>().radius = explosionRadius;
	}

	void Drop()
	{
		if ( Random.Range( 0.0f, 1.0f ) < dropChance )
		{
			Instantiate( dropItem, this.transform.localPosition, this.transform.rotation );
			//Debug.Log( "HEALTH PACK DEPLOYED" );
		}
	}
}
