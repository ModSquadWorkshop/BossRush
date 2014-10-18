using UnityEngine;
using System.Collections;

public class SpiderTankLaserSpin : SpiderTankState
{
	public float rotation;
	public bool spawnMinions;

	[HideInInspector] public SpiderTankState returnState;

	private FlankingSpawner _spawner;

	public override void Awake()
	{
		base.Awake();

		_spawner = GetComponent<FlankingSpawner>();
	}

	void OnEnable()
	{
		_spawner.enabled = spawnMinions;
	}

	void Update()
	{
		spiderTank.laserCanon.PerformPrimaryAttack();
	}

	void FixedUpdate()
	{
		Vector3 angularVelocity = rigidbody.angularVelocity;
		angularVelocity.y = rotation;
		rigidbody.angularVelocity = angularVelocity;
	}

	void Exit()
	{
		enabled = false;
		returnState.enabled = true;
	}
}
