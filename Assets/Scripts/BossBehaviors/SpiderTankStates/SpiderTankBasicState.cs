using UnityEngine;
using System.Collections;

public class SpiderTankBasicState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float minRushInterval, maxRushInterval;

	public PhysicsMovement defaultMovement;

	private SpiderTankRushState _rushState;
	private FlankingSpawner _spawner;

	public override void Awake()
	{
		base.Awake();

		_spawner = GetComponent<FlankingSpawner>();
		_rushState = GetComponent<SpiderTankRushState>();
	}

	void OnEnable()
	{
		spiderTank.mainCanon.SetCooldown( canonDelay );
		_spawner.enabled = true;
		_spawner.amountPerWave = amountPerWave;

		// set initial states of movement scripts
		defaultMovement.enabled = true;
		_rushState.enabled = false;
		_rushState.returnState = this;

		// queue up first rush attack
		Invoke( "StartRush", Random.Range( minRushInterval, maxRushInterval ) );

		// register for health trigger callbacks
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void Update()
	{
		spiderTank.LookMainCanon( turretSpeed );
		spiderTank.FireMainCanon();

		Quaternion lookRotation = Quaternion.LookRotation( player.position - transform.position );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, 90.0f * Time.deltaTime );
	}

	void OnDisable()
	{
		// make sure pending Invokes aren't called while we're disabled
		CancelInvoke();
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
		defaultMovement.enabled = false;
	}

	void StartRush()
	{
		defaultMovement.enabled = false;
		enabled = false;
		_rushState.enabled = true;
	}

	void HealthTriggerCallback( HealthSystem health )
	{
		enabled = false;
		spiderTank.fleeState.returnState = spiderTank.healState;
		spiderTank.fleeState.enabled = true;
	}
}
