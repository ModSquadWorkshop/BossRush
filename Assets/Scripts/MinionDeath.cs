using UnityEngine;
using System.Collections;

public class MinionDeath : MonoBehaviour
{
	public float deathTime;

	public bool dropping;

	DeathTimer timedDeath;
	HealthSystem minionHealth;

	private GameObject _drop;
	public Perk[] drops;

	//[Range( 0.0f, 1.0f )]
	private float _dropGen;

	// Use this for initialization
	void Start ()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( DeathCallback );
		minionHealth = GetComponent<HealthSystem>();
		minionHealth.RegisterHealthCallback( HealthCallback );
		if ( deathTime > 0 )
		{
			timedDeath = gameObject.AddComponent<DeathTimer>();
			timedDeath.deathTime = this.deathTime;
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

	void Drop()
	{
		_dropGen = Random.Range( 0f, 1f );
		Debug.Log( _dropGen );
		foreach ( Perk drop in drops )
		{
			if ( _dropGen >= drop.settings.minChance && _dropGen <= drop.settings.maxChance )
			{
				Debug.Log( "DROP SOMETHING" );
				_drop = Instantiate( drop.gameObject, this.transform.localPosition, this.transform.rotation ) as GameObject;
			}
		}
	}
}
