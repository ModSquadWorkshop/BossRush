using UnityEngine;
using System.Collections;

public class SpiderTankEnterState : SpiderTankState
{
	public EnterStateSettingsList enterStateSettings;
	private EnterStateSettings[] _settings;

	private MoveTowardsTarget _movement;

	void Update()
	{
		if ( ( transform.position - _settings[spiderTank.currentPhase].destination.position ).sqrMagnitude < 1.0f )
		{
			enabled = false;
			spiderTank.basicState.enabled = true;
		}
	}

	public override void Awake()
	{
		base.Awake();

		_movement = GetComponent<MoveTowardsTarget>();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new EnterStateSettings[] { enterStateSettings.phaseOneSettings, 
											   enterStateSettings.phaseTwoSettings, 
											   enterStateSettings.phaseThreeSettings, 
											   enterStateSettings.phaseFourSettings };

		_movement.target = _settings[spiderTank.currentPhase].destination;
		_movement.enabled = true;

		// make sure the boss can walk in/out of the arena.
		Physics.IgnoreCollision( collider, doorCollider, true );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		_movement.enabled = false;
		Physics.IgnoreCollision( collider, doorCollider, false );
	}
}


[System.Serializable]
public class EnterStateSettings
{
	public Transform destination;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class EnterStateSettingsList
{
	public EnterStateSettings phaseOneSettings;
	public EnterStateSettings phaseTwoSettings;
	public EnterStateSettings phaseThreeSettings;
	public EnterStateSettings phaseFourSettings;
}
