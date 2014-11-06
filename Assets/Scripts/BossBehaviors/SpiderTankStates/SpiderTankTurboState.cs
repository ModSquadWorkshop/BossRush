using UnityEngine;
using System.Collections;

public class SpiderTankTurboState : SpiderTankState
{
	public TurboStateSettingsList turboStateSettings;
	private TurboStateSettings[] _settings;

	public NavigateTowardsTarget movementScript;

	void Update()
	{
		spiderTank.LookAllGuns( _settings[spiderTank.currentPhase].turretSpeed );
		spiderTank.FireAllGuns();
	}

	public override void Awake()
	{
		base.Awake();

		movementScript = GetComponent<NavigateTowardsTarget>();
		movementScript.target = player;
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new TurboStateSettings[] { turboStateSettings.phaseOneSettings, 
											   turboStateSettings.phaseTwoSettings, 
											   turboStateSettings.phaseThreeSettings, 
											   turboStateSettings.phaseFourSettings };

		movementScript.enabled = true;

		// register for health trigger callbacks
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );

		Invoke( "TransitionOut", _settings[spiderTank.currentPhase].duration );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		movementScript.enabled = false;
		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void TransitionOut()
	{
		enabled = false;

		if ( Random.value < _settings[spiderTank.currentPhase].laserChance )
		{
			spiderTank.laserSpin.enabled = true;
		}
		else
		{
			spiderTank.basicState.enabled = true;
		}
	}
}


[System.Serializable]
public class TurboStateSettings
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float duration;

	[Range( 0.0f, 1.0f )]
	public float laserChance;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class TurboStateSettingsList
{
	public TurboStateSettings phaseOneSettings;
	public TurboStateSettings phaseTwoSettings;
	public TurboStateSettings phaseThreeSettings;
	public TurboStateSettings phaseFourSettings;
}
