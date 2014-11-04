using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public float mainCanonCooldown;

	public int minionsPerWave;

	[HideInInspector] public SpiderTankState returnState;

	public override void OnEnable()
	{
		base.OnEnable();

		agent.enabled = true;
		agent.SetDestination( shield.transform.position );
		mainCanon.SetCooldown( mainCanonCooldown );
	}

	public void Update()
	{
		spiderTank.LookMainCanon();
		spiderTank.FireMainCanon();

		// check if we're at our destination
		if ( ( transform.position - shield.transform.position ).sqrMagnitude < 1.0f )
		{
			enabled = false;
			spiderTank.healState.enabled = true;
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();

		agent.enabled = false;
	}
}
