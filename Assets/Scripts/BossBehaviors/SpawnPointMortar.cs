using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPointMortar : Mortar
{
	public GameObject spawnerObject;
	[HideInInspector] public SpiderTank _spiderTank;

	public void Init( MortarSettings settings, Vector3 startPos, SpiderTank spiderTank )
	{
		_spiderTank = spiderTank;
		
		List<SpawnPoint> spawnPoints = _spiderTank.spawner.availableSpawnPoints;
		if ( spawnPoints != null )
		{
			if ( spawnPoints.Count > 0 )
			{
				_targetPos = spawnPoints[Random.Range( 0, spawnPoints.Count )].spawnPoint.position;
			}
		}

		base.Init( settings, startPos );
	}

	public override void OnComplete()
	{
		GameObject spawner = Instantiate( spawnerObject, _targetPos, Quaternion.identity ) as GameObject;
		_spiderTank.spawner.AddSpawner( spawner );

		Destroy( gameObject );
		Destroy( _marker );
	}
}
