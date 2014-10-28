using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public int mininionCount;

	void OnEnable()
	{
		spawner.RegisterEnemyCountCallback( MinionCountChange );
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

			spiderTank.enterState.enabled = true;
		}
	}
}
