using UnityEngine;
using System.Collections;

public class SpiderTankTurboState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	private FlankingSpawner _spawner;

	public override void Awake()
	{
		base.Awake();

		_spawner = GetComponent<FlankingSpawner>();
	}

	void OnEnable()
	{
		spiderTank.mainCanon.SetCooldown( canonDelay );
		_spawner.enabled = true;
		_spawner.amountPerWave = amountPerWave;
	}

	void Update()
	{
		spiderTank.LookAllGuns( turretSpeed );
		spiderTank.FireAllGuns();
	}

	void OnDisable()
	{

	}
}
