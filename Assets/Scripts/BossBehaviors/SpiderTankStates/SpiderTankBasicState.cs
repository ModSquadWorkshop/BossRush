using UnityEngine;
using System.Collections;

public class SpiderTankBasicState : SpiderTankState
{
	void OnEnable()
	{

	}

	void Update()
	{
		// have main cannon track player with delay
		Quaternion look = Quaternion.LookRotation( spiderTank.player.position + Vector3.up * 2 - spiderTank.mainCannon.transform.position );
		spiderTank.mainCannon.transform.rotation = Quaternion.Lerp( spiderTank.mainCannon.transform.rotation, look, spiderTank.turretSpeed * Time.deltaTime );

		if ( !spiderTank.mainCannon.IsOnCooldown )
		{
			spiderTank.mainCannon.PerformPrimaryAttack();
		}
	}

	void OnDisable()
	{

	}
}
