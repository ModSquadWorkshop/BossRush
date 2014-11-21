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

		base.Init( settings, startPos );
	}

	public override void OnComplete()
	{
		GameObject spawner = Instantiate( spawnerObject, _targetPos, Quaternion.identity ) as GameObject;
		_spiderTank.spawner.AddSpawner( spawner, _spawn );

		Destroy( gameObject );
		Destroy( _marker );
	}

	protected override Vector3 GetTargetPosition()
	{
		if ( _spawn != null )
		{
			return _spawn.transform.position;
		}

		return base.GetTargetPosition();
	}
}
