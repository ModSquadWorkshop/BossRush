using UnityEngine;
using System.Collections;

public class MinionDeath : MonoBehaviour
{
	public float deathTime;

	public bool dropping;

	HealthSystem minionHealth;

	public Perk[] drops;

	// Use this for initialization
	void Awake()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( DeathCallback );
		minionHealth = GetComponent<HealthSystem>();
		minionHealth.RegisterHealthCallback( HealthCallback );
	}

	void Start()
	{
		if ( deathTime > 0.0f )
		{
			Invoke( "DestroySelf", deathTime );
		}
	}

	void HealthCallback( HealthSystem minionHealth, float change )
	{
		if ( dropping && minionHealth.health <= 0 )
		{
			Drop();
		}
	}

	void DeathCallback( GameObject minion )
	{
		// ensure that we don't get notified of our death multiple times
		GetComponent<DeathSystem>().DeregisterDeathCallback( DeathCallback );
	}

	void DestroySelf()
	{
		GetComponent<DeathSystem>().Kill();
	}

	void Drop()
	{
		float dropGen = Random.Range( 0.0f, 1.0f );
		float cumulativeChance = 0.0f;
		foreach ( Perk drop in drops )
		{
			cumulativeChance += drop.dropChance;
			if ( cumulativeChance > dropGen )
			{
				Instantiate( drop.gameObject, transform.localPosition, transform.rotation );
				return;
			}
		}
	}
}
