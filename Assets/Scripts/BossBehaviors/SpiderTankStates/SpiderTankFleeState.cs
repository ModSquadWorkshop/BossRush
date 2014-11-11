using UnityEngine;
using System.Collections;

public class SpiderTankFleeState : SpiderTankState
{
	public FleeStateSettingsList fleeStateSettings;
	private FleeStateSettings[] _settings;

	[HideInInspector] public SpiderTankState returnState;

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

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new FleeStateSettings[] { fleeStateSettings.phaseOneSettings, 
											  fleeStateSettings.phaseTwoSettings, 
											  fleeStateSettings.phaseThreeSettings, 
											  fleeStateSettings.phaseFourSettings };

		agent.enabled = true;
		agent.SetDestination( shield.transform.position );
		mainCanon.SetCooldown( _settings[spiderTank.currentPhase].mainCanonCooldown );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		agent.enabled = false;
	}
}


[System.Serializable]
public class FleeStateSettings
{
	public float mainCanonCooldown;
	public int minionsPerWave;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class FleeStateSettingsList
{
	public FleeStateSettings phaseOneSettings;
	public FleeStateSettings phaseTwoSettings;
	public FleeStateSettings phaseThreeSettings;
	public FleeStateSettings phaseFourSettings;
}
