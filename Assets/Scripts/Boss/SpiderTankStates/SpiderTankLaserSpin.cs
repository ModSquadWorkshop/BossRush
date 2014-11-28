using UnityEngine;
using System.Collections;

public class SpiderTankLaserSpin : SpiderTankState
{
	public AudioClip BossLaser;
	public LaserSpinSettingsList laserSpinSettings;
	private LaserSpinSettings[] _settings;

	void Update()
	{
		for ( int i = 0; i < spiderTank.laserCanon.Length; ++i )
		{
			spiderTank.laserCanon[i].PerformPrimaryAttack();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new LaserSpinSettings[] { laserSpinSettings.phaseOneSettings, 
											  laserSpinSettings.phaseTwoSettings, 
											  laserSpinSettings.phaseThreeSettings, 
											  laserSpinSettings.phaseFourSettings };
		audio.clip = BossLaser;
		audio.Play();
		audio.volume = .7f;
		audio.priority = 0;

		Invoke( "TransitionOut", _settings[spiderTank.currentPhase].duration );
		spiderTank.RegisterHealthTriggerCallback( HealthTriggerCallback );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		spiderTank.DeregisterHealthTriggerCallback( HealthTriggerCallback );
	}

	void FixedUpdate()
	{
		Vector3 angularVelocity = rigidbody.angularVelocity;
		angularVelocity.y = _settings[spiderTank.currentPhase].rotation;
		rigidbody.angularVelocity = angularVelocity;
	}

	void TransitionOut()
	{
		enabled = false;

		if ( Random.value < 0.5f )
		{
			spiderTank.basicState.enabled = true;
		}
		else
		{
			spiderTank.turboState.enabled = true;
		}
	}
}


[System.Serializable]
public class LaserSpinSettings
{
	public float rotation;
	public float duration;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class LaserSpinSettingsList
{
	public LaserSpinSettings phaseOneSettings;
	public LaserSpinSettings phaseTwoSettings;
	public LaserSpinSettings phaseThreeSettings;
	public LaserSpinSettings phaseFourSettings;
}
