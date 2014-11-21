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

		SpawnPoint spawn = _spiderTank.spawner.GetRandomAvailableSpawnPoint();
		if ( spawn != null )
		{
			_targetPos = spawn.spawnPoint.position;
			spawn.available = false;
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
