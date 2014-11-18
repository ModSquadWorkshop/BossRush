using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : MonoBehaviour
{
	public GameObject explosionEffect;

	public float explosionRadius;
	public float explosionDamage;
	public float explosionForce;

	void Awake()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( Explode );
	}

	void Explode( GameObject obj )
	{
		Collider[] collisions = Physics.OverlapSphere( transform.position, explosionRadius );
		foreach ( Collider col in collisions )
		{
			HealthSystem otherHealth = col.gameObject.GetComponent<HealthSystem>();
			if ( otherHealth != null )
			{
				otherHealth.Damage( explosionDamage );
			}

			if ( col.rigidbody != null )
			{
				col.rigidbody.AddExplosionForce( explosionForce, transform.position, explosionRadius );
			}
		}

		Instantiate( explosionEffect, transform.position, transform.rotation );//.GetComponent<Explosion>().radius = explosionRadius;
	}
}
