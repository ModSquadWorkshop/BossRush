using UnityEngine;
using System.Collections;

public class SpawnPointMortar : Mortar
{
	public GameObject spawnerObject;
	[HideInInspector] public SpiderTank _spiderTank;

	public void Init( MortarSettings settings, Vector3 startPos, SpiderTank spiderTank )
	{
		base.Init( settings, startPos );
		_spiderTank = spiderTank;
	}

	public override void OnComplete()
	{
		GameObject spawner = Instantiate( spawnerObject, _targetPos, Quaternion.identity ) as GameObject;
		_spiderTank.spawner.AddSpawner( spawner );

		Destroy( gameObject );
		Destroy( _marker );
	}
}
