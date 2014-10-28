using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public float mainCanonCooldown;

	public int minionsPerWave;

	[HideInInspector] public SpiderTankState returnState;

	public void OnEnable()
	{
		agent.enabled = true;
		agent.SetDestination( shield.transform.position );
		mainCanon.SetCooldown( mainCanonCooldown );

		spawner.enabled = true;
		spawner.amountPerWave = minionsPerWave;
		spawner.StartSpawning();
	}

	public void Update()
	{
		spiderTank.LookMainCanon();
		spiderTank.FireMainCanon();

		// check if we're at we're destination
		if ( ( transform.position - shield.transform.position ).sqrMagnitude < 1.0f )
		{
			enabled = false;
			spiderTank.healState.enabled = true;
		}
	}

	public void OnDisable()
	{
		spawner.enabled = false;
		agent.enabled = false;
		spawner.StopSpawning();
	}
}
