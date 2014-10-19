using UnityEngine;
using System.Collections;

public class SpiderTankHealState : SpiderTankState
{
	public EnemySpawner spawner;
	public int mininionCount;

	public float healRate;

	public override void Awake()
	{
		base.Awake();

		spawner.RegisterEnemyCountCallback( new EnemySpawner.EnemyCountChange( MinionCountChange ) );
	}

	void OnEnable()
	{
		spawner.enabled = true;
		spawner.Spawn( mininionCount );
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
		if ( enabled && count == 0 )
		{
			enabled = false;
			spawner.enabled = false;

			spiderTank.SetDamageBase();

			spiderTank.fleeState.returnState = spiderTank.basicState;
			spiderTank.fleeState.enabled = true;
		}
	}
}
