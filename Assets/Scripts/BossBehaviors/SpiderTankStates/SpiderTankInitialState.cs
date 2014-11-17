using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public Transform fallPoint;
	public float fallTime;

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
			Hashtable settings = new Hashtable();
			settings.Add( "position", fallPoint );
			settings.Add( "time", fallTime );
			settings.Add( "easetype", iTween.EaseType.linear );
			iTween.MoveTo( gameObject, settings );

			Invoke( "FallEnded", fallTime );
		}
	}

	public void FallEnded()
	{
		enabled = false;
		spawner.enabled = false;
		//_animator.enabled = false;

		spiderTank.basicState.enabled = true;
	}
}
