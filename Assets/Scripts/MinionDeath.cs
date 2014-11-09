using UnityEngine;
using System.Collections;

public class MinionDeath : MonoBehaviour
{
	public float deathTime;

	public bool dropping;

	DeathTimer timedDeath;

	public GameObject dropItem;

	[Range( 0.0f, 1.0f )]
	public float dropChance;

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

		if ( dropping )
		{
			Drop();
		}
	}

	void Drop()
	{
		if ( Random.Range( 0.0f, 1.0f ) < dropChance )
		{
			Instantiate( dropItem, this.transform.localPosition, this.transform.rotation );
			Debug.Log( "HEALTH PACK DEPLOYED" );
		}
	}
}
