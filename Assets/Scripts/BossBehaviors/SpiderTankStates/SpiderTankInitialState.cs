using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public Transform[] fallPoints;
	public float preFallDelay;
	public float fallTime;
	public float postFallDelay;

	public GameObject explodeMinion;
	public int numMinions;
	public float maxWaitTime;

	private Animator _animator;

	public override void Awake()
	{
		base.Awake();

		_animator = GetComponent<Animator>();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		spawner.RegisterEnemyCountCallback( MinionCountChange );
		spawner.Spawn( numMinions, explodeMinion );

		Invoke( "Exit", maxWaitTime );
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
			// move to be above destination
			Transform destination = findClosestToPlayer();
			transform.position = destination.position + new Vector3( 0.0f, 200.0f, 0.0f );

			// start fal
			Hashtable settings = new Hashtable();
			settings.Add( "delay", preFallDelay );
			settings.Add( "position", destination );
			settings.Add( "time", fallTime );
			settings.Add( "easetype", iTween.EaseType.linear );
			iTween.MoveTo( gameObject, settings );

			Invoke( "FallEnded", preFallDelay + fallTime + postFallDelay );
		}
	}

	public void FallEnded()
	{
		enabled = false;
		spawner.enabled = false;
		//_animator.enabled = false;

		spiderTank.basicState.enabled = true;
	}

	public Transform findClosestToPlayer()
	{
		Transform closest = fallPoints[0];
		for ( int index = 1; index < fallPoints.Length; index++ )
		{
			if ( ( player.position - fallPoints[index].position ).sqrMagnitude < ( player.position - closest.position ).sqrMagnitude )
			{
				closest = fallPoints[index];
			}
		}
		return closest;
	}
}
