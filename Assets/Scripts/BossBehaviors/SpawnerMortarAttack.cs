using UnityEngine;
using System.Collections;

public class SpawnerMortarAttack : MortarAttack
{
	[HideInInspector] public SpiderTank spiderTank = null;

	protected override IEnumerator LaunchMortar( int numMortars )
	{
		while ( numMortars > 0 && spiderTank.spawner.spawners.Count < spiderTank.spawner.maxSpawnPoints )
		{
			SpawnPointMortar mortarObject = ( Instantiate( mortar ) as GameObject ).GetComponent<SpawnPointMortar>();
			mortarObject.Init( mortarSettings, transform.position, spiderTank );
			numMortars--;

			yield return new WaitForSeconds( delayBetweenMortars );
		}

		_firing = false;
	}
}
