using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public int mininionCount;

	public override void Awake()
	{
		base.Awake();

		spawner.RegisterEnemyCountCallback( MinionCountChange );
	}

	void OnEnable()
	{
		spawner.enabled = true;
		spawner.Spawn( mininionCount );
	}

	void OnDisable()
	{
		spawner.DeregisterEnemyCountCallback( MinionCountChange );
		spiderTank.SetDamageBase();
	}

	public void MinionCountChange( int count )
	{
		if ( enabled && count == 0 )
		{
			enabled = false;
			spawner.enabled = false;

			spiderTank.fleeState.returnState = spiderTank.basicState;
			spiderTank.fleeState.enabled = true;
		}
	}
}
