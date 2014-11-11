using UnityEngine;
using System.Collections;

public class SpiderTankInitialState : SpiderTankState
{
	public InitialStateSettingsList initialStateSettings;
	private InitialStateSettings[] _settings;

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new InitialStateSettings[] { initialStateSettings.phaseOneSettings, 
												 initialStateSettings.phaseTwoSettings, 
												 initialStateSettings.phaseThreeSettings, 
												 initialStateSettings.phaseFourSettings };

		spawner.RegisterEnemyCountCallback( MinionCountChange );
		spawner.Spawn( _settings[spiderTank.currentPhase].mininionCount );
	}

	public override void OnDisable()
	{
		base.OnDisable();

		spawner.DeregisterEnemyCountCallback( MinionCountChange );
		spiderTank.SetDamageBase();
	}

	public void MinionCountChange( int count )
	{
		if ( enabled && count == 0 )
		{
			enabled = false;
			spawner.enabled = false;

			spiderTank.enterState.enabled = true;
		}
	}
}


[System.Serializable]
public class InitialStateSettings
{
	public int mininionCount;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class InitialStateSettingsList
{
	public InitialStateSettings phaseOneSettings;
	public InitialStateSettings phaseTwoSettings;
	public InitialStateSettings phaseThreeSettings;
	public InitialStateSettings phaseFourSettings;
}
