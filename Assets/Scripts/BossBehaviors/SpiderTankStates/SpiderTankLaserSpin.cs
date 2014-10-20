using UnityEngine;
using System.Collections;

public class SpiderTankLaserSpin : SpiderTankState
{
	public float rotation;
	public bool spawnMinions;

	public float duration;

	private FlankingSpawner _spawner;

	public override void Awake()
	{
		base.Awake();

		_spawner = GetComponent<FlankingSpawner>();
	}

	void OnEnable()
	{
		_spawner.enabled = spawnMinions;
		Invoke( "TransitionOut", duration );
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void Update()
	{
		spiderTank.laserCanon.PerformPrimaryAttack();
	}

	void OnDisable()
	{
		_spawner.enabled = false;
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void FixedUpdate()
	{
		Vector3 angularVelocity = rigidbody.angularVelocity;
		angularVelocity.y = rotation;
		rigidbody.angularVelocity = angularVelocity;
	}

	void TransitionOut()
	{
		enabled = false;
		spiderTank.basicState.enabled = true;
	}
}
