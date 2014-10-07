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
		spiderTank.mainCannon.SetCooldown( canonDelay );
		_spawner.enabled = true;
		_spawner.amountPerWave = amountPerWave;
	}

	void Update()
	{
		// have main cannon track player with delay
		Quaternion look = Quaternion.LookRotation( spiderTank.player.position + Vector3.up * 2 - spiderTank.mainCannon.transform.position );
		spiderTank.mainCannon.transform.rotation = Quaternion.Lerp( spiderTank.mainCannon.transform.rotation, look, turretSpeed * Time.deltaTime );

		if ( !spiderTank.mainCannon.IsOnCooldown )
		{
			spiderTank.mainCannon.PerformPrimaryAttack();
		}
	}

	void OnDisable()
	{

	}
}
