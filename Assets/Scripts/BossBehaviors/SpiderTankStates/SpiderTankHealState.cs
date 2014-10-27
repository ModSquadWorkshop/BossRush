using UnityEngine;
using System.Collections;

public class SpiderTankHealState : SpiderTankState
{
	public int mininionCount;

	public float healRate;

	public override void Awake()
	{
		base.Awake();

		arenaSpawner.RegisterEnemyCountCallback( MinionCountChange );
	}

	void OnEnable()
	{
		arenaSpawner.Spawn( mininionCount );
	}

	public void Update()
	{
		spiderTank.health.Heal( healRate * Time.deltaTime );
		if ( spiderTank.health.atMaxHealth )
		{
			MinionCountChange( 0 ); // kinda janky, but whatevs
		}
	}

	public void MinionCountChange( int count )
	{
		if ( this != null && enabled && count == 0 )
		{
			enabled = false;
			arenaSpawner.enabled = false;

			spiderTank.SetDamageBase();

			spiderTank.fleeState.returnState = spiderTank.basicState;
			spiderTank.fleeState.enabled = true;
		}
	}
}
