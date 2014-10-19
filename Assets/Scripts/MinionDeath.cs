using UnityEngine;
using System.Collections;

public class MinionDeath : MonoBehaviour
{
	public float deathTime;

	public bool exploding;
	public bool dropping;

	HealthSystem otherHealth;
	DeathTimer timedDeath;
	DeathTimer effectDeath;

	public GameObject healthDrop; //prefab for health pack perk

	[Range( 0.0f, 1.0f )]
	public float dropChance;

	public float explosionRadius;
	public float explosionDamage;
	public Material explosionMat;

	// Use this for initialization
	void Start ()
	{
		GetComponent<DeathSystem>().RegisterDeathCallback( TargetDeathCallback );
		if ( deathTime > 0 )
		{
			timedDeath = gameObject.AddComponent<DeathTimer>();
			timedDeath.deathTime = this.deathTime;
		}
	}

	void TargetDeathCallback( GameObject minion )
	{
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
		//Debug.Log( "EXPLODE MINION" );
		Collider[] col = Physics.OverlapSphere( this.transform.position, explosionRadius );

		//Create a visual representation of explosion
		GameObject expl = GameObject.CreatePrimitive( PrimitiveType.Sphere );
		expl.transform.position = this.transform.localPosition;
		expl.transform.localScale = new Vector3( explosionRadius, explosionRadius, explosionRadius );
		expl.renderer.material = explosionMat;
		expl.collider.enabled = false;
		effectDeath = expl.AddComponent<DeathTimer>();
		effectDeath.deathTime = 1f;

		int i = 0;
		while( i < col.Length )
		{
			otherHealth = col[i].gameObject.GetComponent<HealthSystem>();
			if( otherHealth != null )
			{
				//Debug.Log( "OTHER TAKING EXPLOSION DAMAGE" );
				otherHealth.health -= explosionDamage;
			}
			++i;
		}
	}

	void Drop()
	{
		if ( Random.Range( 0.0f, 1.0f ) < dropChance )
		{
			Instantiate( healthDrop, this.transform.localPosition, this.transform.rotation );
			Debug.Log( "HEALTH PACK DEPLOYED" );
		}
	}
}
