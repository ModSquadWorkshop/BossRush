using UnityEngine;
using System.Collections;

public class ExplodeOnDeath : MonoBehaviour
{
	public GameObject explosionEffect;

	public float explosionRadius;
	public float explosionDamage;
	public float explosionForce;

	private DamageSystem _damageSystem;

	void Start()
	{
		_damageSystem = GetComponent<DamageSystem>();
	}

	void Awake()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( Explode );
	}

	void Explode( GameObject obj )
	{
		Collider[] collisions = Physics.OverlapSphere( transform.position, explosionRadius );
		foreach ( Collider col in collisions )
		{
			if ( _damageSystem.IsTarget( col.tag ) )
			{
				DealDamage( col.gameObject );

				if ( col.rigidbody != null )
				{
					col.rigidbody.AddExplosionForce( explosionForce, transform.position, explosionRadius );
				}
			}
		}

		Instantiate( explosionEffect, transform.position, transform.rotation );//.GetComponent<Explosion>().radius = explosionRadius;
	}

	void DealDamage( GameObject target )
	{
		HealthSystem healthSystem = target.gameObject.GetComponent<HealthSystem>();
		if ( healthSystem != null )
		{
			healthSystem.Damage( explosionDamage );
		}
	}
}
