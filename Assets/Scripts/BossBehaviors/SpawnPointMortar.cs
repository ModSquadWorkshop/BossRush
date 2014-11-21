using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPointMortar : Mortar
{
	public GameObject spawnerObject;
	[HideInInspector] public SpiderTank _spiderTank;

	private SpawnPoint _spawn;

	public void Init( MortarSettings settings, Vector3 startPos, SpiderTank spiderTank )
	{
		_spiderTank = spiderTank;
		_spawn = _spiderTank.spawner.GetRandomAvailableSpawnPoint();

		if ( _spawn != null )
		{
			_targetPos = _spawn.spawnPoint.position;
			_spawn.available = false;
		}

		base.Init( settings, startPos );
	}

	public override void OnComplete()
	{
		GameObject spawner = Instantiate( spawnerObject, _targetPos, Quaternion.identity ) as GameObject;
		_spiderTank.spawner.AddSpawner( spawner, _spawn );

		Destroy( gameObject );
		Destroy( _marker );
	}
}
