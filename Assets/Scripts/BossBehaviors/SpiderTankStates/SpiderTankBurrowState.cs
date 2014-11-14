using UnityEngine;
using System.Collections;

public class SpiderTankBurrowState : SpiderTankState 
{
	public Transform relocation;

	public BurrowStateSettingsList burrowStateSettings;
	private BurrowStateSettings[] _settings;

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new BurrowStateSettings[] { burrowStateSettings.phaseOneSettings, 
												burrowStateSettings.phaseTwoSettings, 
												burrowStateSettings.phaseThreeSettings, 
												burrowStateSettings.phaseFourSettings };

		DoBurrowAnimation();
	}

	void DoBurrowAnimation()
	{
		//.. do stuff
		OnBurrowAnimationComplete(); // temp
	}

	void DoUnburrowAnimation()
	{
		//.. do stuff
		OnUnburrowAnimationComplete(); // temp
	}

	void OnBurrowAnimationComplete()
	{
		// move the spider tank to its new position
		spiderTank.transform.position = relocation.transform.position;

		// start the unburrow sequence after x time underground
		Invoke( "DoUnburrowAnimation", _settings[spiderTank.currentPhase].timeUnderground );
	}

	void OnUnburrowAnimationComplete()
	{
		//... do stuff, if necessary
	}
}


[System.Serializable]
public class BurrowStateSettings
{
	public float timeUnderground;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class BurrowStateSettingsList
{
	public BurrowStateSettings phaseOneSettings;
	public BurrowStateSettings phaseTwoSettings;
	public BurrowStateSettings phaseThreeSettings;
	public BurrowStateSettings phaseFourSettings;
}
