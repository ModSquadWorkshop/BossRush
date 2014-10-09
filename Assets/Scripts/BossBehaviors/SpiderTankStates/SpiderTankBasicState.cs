using UnityEngine;
using System.Collections;

public class SpiderTankBasicState : SpiderTankState
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	private FlankingSpawner _spawner;

	void Awake()
	{
		_spawner = GetComponent<FlankingSpawner>();
	}

	void OnEnable()
	{
		Debug.Log( "setting canon cooldown to: " + canonDelay );
		spiderTank.mainCanon.SetCooldown( canonDelay );
		_spawner.enabled = true;
		_spawner.amountPerWave = amountPerWave;
	}

	void Update()
	{
		spiderTank.LookMainCanon( turretSpeed );
		spiderTank.FireMainCanon();
	}

	void OnDisable()
	{

	}
}
