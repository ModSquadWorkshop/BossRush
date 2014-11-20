using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public GameObject fallPointsRoot;
	public float preFallDelay;
	public float fallTime;
	public float postFallDelay;
	public GameObject landingEffect;

	public GameObject explodeMinion;
	public int numMinions;
	public float maxWaitTime;

	private Transform[] _fallPoints;

	public override void Awake()
	{
		base.Awake();

		// get the list of fall points
		_fallPoints = fallPointsRoot.GetComponentsInChildren<Transform>();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		spawner.RegisterEnemyCountCallback( MinionCountChange );
		spawner.Spawn( numMinions, explodeMinion );

		Invoke( "StartFall", maxWaitTime );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		spawner.DeregisterEnemyCountCallback( MinionCountChange );
		spiderTank.SetDamageBase();
	}

	public void MinionCountChange( int count )
	{
		if ( enabled && count == 0 )
		{
			StartFall();
		}
	}

	void StartFall()
	{
		CancelInvoke();

		// move to be above destination
		Transform destination = findClosestToPlayer();
		transform.position = destination.position + new Vector3( 0.0f, 200.0f, 0.0f );

		// start fall
		Hashtable settings = new Hashtable();
		settings.Add( "delay", preFallDelay );
		settings.Add( "position", destination );
		settings.Add( "time", fallTime );
		settings.Add( "easetype", iTween.EaseType.linear );
		iTween.MoveTo( gameObject, settings );

		Invoke( "FallEnded", preFallDelay + fallTime );
	}

	void FallEnded()
	{
		Instantiate( landingEffect, transform.position, Quaternion.identity );
		Invoke( "Exit", postFallDelay );
	}

	void Exit()
	{
		enabled = false;
		spawner.enabled = false;

		spiderTank.basicState.enabled = true;
	}

	public Transform findClosestToPlayer()
	{
		Transform closest = _fallPoints[0];
		for ( int index = 1; index < _fallPoints.Length; index++ )
		{
			if ( ( player.position - _fallPoints[index].position ).sqrMagnitude < ( player.position - closest.position ).sqrMagnitude )
			{
				closest = _fallPoints[index];
			}
		}
		return closest;
	}
}
