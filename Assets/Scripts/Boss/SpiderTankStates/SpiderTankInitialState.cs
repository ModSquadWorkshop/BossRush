using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public LevelManager levelManager;

	public GameObject fallPointsRoot;
	public float preFallDelay;
	public float fallTime;
	public float impactShake;
	public float postFallDelay;
	public float minFallDistance;
	public GameObject landingEffect;
	public AudioClip fallingSound;
	public AudioClip landingSound;

	public GameObject explodeMinion;
	public int numMinions;
	public float maxWaitTime;

	public int initialSpawnersAmount;
	public float delayBeforeMinionSpawning;

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

		spawnerLauncher.Launch( initialSpawnersAmount );

		Invoke( "SpawnersFallEnded", delayBeforeMinionSpawning );
	}

	public override void OnDisable()
	{
		base.OnDisable();
		spiderTank.SetDamageBase();
	}

	public void MinionCountChange( int count )
	{
		if ( enabled && count == 0 )
		{
			CancelInvoke();
			PreFall();
		}
	}

	void PreFall()
	{
		spawner.DeregisterEnemyCountCallback( MinionCountChange );
		Invoke( "StartFall", preFallDelay );
	}

	private void StartFall()
	{
		// move to be above destination
		Transform destination = findClosestToPlayer();
		transform.position = destination.position + new Vector3( 0.0f, 200.0f, 0.0f );

		// start fall
		Hashtable settings = new Hashtable();
		settings.Add( "position", destination );
		settings.Add( "time", fallTime );
		settings.Add( "easetype", iTween.EaseType.linear );
		iTween.MoveTo( gameObject, settings );

		audio.clip = fallingSound;
		audio.Play();
		audio.volume = .9f;
		audio.priority = 0;

		Invoke( "FallEnded", fallTime );
	}

	private void FallEnded()
	{
		Instantiate( landingEffect, transform.position, Quaternion.identity );
		audio.clip = landingSound;
		audio.Play();
		audio.volume = .8f;
		audio.priority = 0;
		Camera.main.GetComponentInChildren<CameraShake>().Shake( impactShake );
		levelManager.ShowBossHealthDisplay();

		Invoke( "Exit", postFallDelay );
	}

	private void SpawnersFallEnded()
	{
		spawner.RegisterEnemyCountCallback( MinionCountChange );
		spawner.Spawn( numMinions, explodeMinion );

		Invoke( "PreFall", maxWaitTime );
	}

	private void Exit()
	{
		enabled = false;
		spiderTank.basicState.enabled = true;
	}

	private Transform findClosestToPlayer()
	{
		Transform closest = _fallPoints[0];
		float closestDistance = (player.position - closest.position).sqrMagnitude;

		for ( int index = 1; index < _fallPoints.Length; index++ )
		{
			float distance = (player.position - _fallPoints[index].position).sqrMagnitude;
			if ( distance < closestDistance && distance >= minFallDistance )
			{
				closest = _fallPoints[index];
				closestDistance = distance;
			}
		}

		return closest;
	}
}
