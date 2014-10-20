using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public int mininionCount;

	public override void Awake()
	{
		base.Awake();

		arenaSpawner.RegisterEnemyCountCallback( MinionCountChange );
	}

	void OnEnable()
	{
		arenaSpawner.enabled = true;
		arenaSpawner.Spawn( mininionCount );
	}

	public void MinionCountChange( int count )
	{
		if ( enabled && count == 0 )
		{
			enabled = false;
			arenaSpawner.enabled = false;

			spiderTank.fleeState.returnState = spiderTank.basicState;
			spiderTank.fleeState.enabled = true;
		}
	}
}
