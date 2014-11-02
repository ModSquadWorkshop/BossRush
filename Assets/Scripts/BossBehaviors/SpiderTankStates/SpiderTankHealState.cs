using UnityEngine;
using System.Collections;

public class SpiderTankHealState : SpiderTankState
{
	public int mininionCount;
	public float healRate;

	public bool endWhenNoMinionsLeft;

	public override void Awake()
	{
		base.Awake();
	}

	void OnEnable()
	{
		if ( endWhenNoMinionsLeft )
		{
			spawner.RegisterEnemyCountCallback( MinionCountChange );
		}
		else
		{
			spawner.enabled = true;
		}
		spawner.Spawn( mininionCount );

		shield.SetActive( true );
		shield.gameObject.GetComponent<DeathSystem>().RegisterDeathCallback( ShieldDestroyed );

		Physics.IgnoreCollision( collider, shield.collider, true );
	}

	public void Update()
	{
		spiderTank.health.Heal( healRate * Time.deltaTime );
		if ( spiderTank.health.atMaxHealth )
		{
			MinionCountChange( 0 ); // kinda janky, but whatevs
		}
	}

	public void OnDisable()
	{
		spawner.enabled = false;
		spiderTank.SetDamageBase();
		spawner.DeregisterEnemyCountCallback( MinionCountChange );
		shield.SetActive( false );
		shield.gameObject.GetComponent<DeathSystem>().DeregisterDeathCallback( ShieldDestroyed );
	}

	public void MinionCountChange( int count )
	{
		if ( this != null && enabled && count == 0 )
		{
			enabled = false;
			spiderTank.basicState.enabled = true;
		}
	}

	public void ShieldDestroyed( GameObject shield )
	{
		enabled = false;
		spiderTank.basicState.enabled = true;
	}
}
