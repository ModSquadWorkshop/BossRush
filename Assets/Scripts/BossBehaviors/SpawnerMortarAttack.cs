using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerMortarAttack : MortarAttack
{
	[HideInInspector] public SpiderTank spiderTank = null;

	private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();

	void Awake()
	{
		// Get a list of spawn points from the targets
		foreach ( Transform target in mortarSettings.targets )
		{
			SpawnPoint spawn = target.GetComponent<SpawnPoint>();
			if ( spawn != null )
			{
				_spawnPoints.Add( spawn );
			}
		}
	}

	protected override IEnumerator LaunchMortar( int numMortars )
	{
		while ( numMortars > 0 && spiderTank.spawner.spawners.Count < spiderTank.spawner.settings.maxSpawnPoints )
		{
			yield return new WaitForSeconds( delayBetweenMortars );

			SpawnPointMortar mortarObject = ( Instantiate( mortar ) as GameObject ).GetComponent<SpawnPointMortar>();
			mortarObject.Init( mortarSettings, transform.position, GetRandomAvailableSpawnPoint(), spiderTank );
			numMortars--;
		}

		_firing = false;
	}

	public SpawnPoint GetRandomAvailableSpawnPoint()
	{
		List<SpawnPoint> availableSpawnPoints = new List<SpawnPoint>();

		// get all available spawn points
		for ( int i = 0; i < _spawnPoints.Count; i++ )
		{
			SpawnPoint spawn = _spawnPoints[i];
			if ( spawn.available )
			{
				availableSpawnPoints.Add( spawn );
			}
		}

		// pick one at random
		return availableSpawnPoints[Random.Range( 0, availableSpawnPoints.Count )];
	}
}
