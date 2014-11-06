using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState
{
	public int amountOfMortars;
	public float launchInterval;

	public override void OnEnable()
	{
		base.OnEnable();
		StartLaunchAtInterval( amountOfMortars, launchInterval );
	}
}
