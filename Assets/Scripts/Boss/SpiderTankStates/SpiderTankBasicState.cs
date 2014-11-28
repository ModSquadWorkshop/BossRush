using UnityEngine;
using System.Collections;

public class SpiderTankBasicState : SpiderTankState
{
	public BasicStateSettingsList basicStateSettings;
	private BasicStateSettings[] _settings;

	public PhysicsMovement movementScript;

	void Update()
	{
		spiderTank.LookMainCanon( _settings[spiderTank.currentPhase].turretSpeed );
		spiderTank.BeginMainCanon();

		Quaternion lookRotation = Quaternion.LookRotation( player.position - transform.position );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, 90.0f * Time.deltaTime );
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new BasicStateSettings[] { basicStateSettings.phaseOneSettings,
		                                       basicStateSettings.phaseTwoSettings,
		                                       basicStateSettings.phaseThreeSettings,
		                                       basicStateSettings.phaseFourSettings
		                                     };

		spiderTank.mainCanon.SetCooldown( _settings[spiderTank.currentPhase].canonDelay );

		// set initial states of movement scripts
		movementScript.enabled = true;
		spiderTank.rushState.returnState = this;

		// queue up first rush attack
		Invoke( "TransitionOut", Random.Range( _settings[spiderTank.currentPhase].minRushInterval,
		                                       _settings[spiderTank.currentPhase].maxRushInterval ) );

		// register for health trigger callbacks
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
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

		if ( Random.value < _settings[spiderTank.currentPhase].turboChance )
		{
			spiderTank.turboState.enabled = true;
		}
		else
		{
			spiderTank.rushState.enabled = true;
		}
	}
}


[System.Serializable]
public class BasicStateSettings
{
	public float turretSpeed;
	public float canonDelay;
	public int amountPerWave;

	public float minRushInterval, maxRushInterval;

	[Range( 0.0f, 1.0f )]
	public float turboChance;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class BasicStateSettingsList
{
	public BasicStateSettings phaseOneSettings;
	public BasicStateSettings phaseTwoSettings;
	public BasicStateSettings phaseThreeSettings;
	public BasicStateSettings phaseFourSettings;
}
