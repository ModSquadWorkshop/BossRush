using UnityEngine;
using System.Collections;

public class SpawnPointMortar : Mortar
{
	public GameObject spawnerObject;
	public SpiderTank _spiderTank;

	public void Init( MortarSettings settings, Vector3 startPos, SpiderTank spiderTank )
	{
		base.Init( settings, startPos );
		_spiderTank = spiderTank;
	}

	public override void OnComplete()
	{
		GameObject spawnPoint = Instantiate( spawnerObject, _targetPos, Quaternion.identity ) as GameObject;
		_spiderTank.spawner.AddSpawnPoint( spawnPoint.transform );

		Destroy( gameObject );
		Destroy( _marker );
	}
}
