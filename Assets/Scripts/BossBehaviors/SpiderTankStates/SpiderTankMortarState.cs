using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState
{
	public MortarStateSettingsList mortarStateSettings;
	private MortarStateSettings[] _settings;

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new MortarStateSettings[] { mortarStateSettings.phaseOneSettings, 
												mortarStateSettings.phaseTwoSettings, 
												mortarStateSettings.phaseThreeSettings, 
												mortarStateSettings.phaseFourSettings };

		StartLaunchAtInterval( _settings[spiderTank.currentPhase].amountOfMortars, 
							   _settings[spiderTank.currentPhase].launchInterval );
	}
}


[System.Serializable]
public class MortarStateSettings
{
	public int amountOfMortars;
	public float launchInterval;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class MortarStateSettingsList
{
	public MortarStateSettings phaseOneSettings;
	public MortarStateSettings phaseTwoSettings;
	public MortarStateSettings phaseThreeSettings;
	public MortarStateSettings phaseFourSettings;
}
