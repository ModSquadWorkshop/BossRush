using UnityEngine;
using System.Collections;

public class SpiderTankBurrowState : SpiderTankState 
{
	public Transform[] relocationPoints;

	public BurrowStateSettingsList burrowStateSettings;
	private BurrowStateSettings[] _settings;

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new BurrowStateSettings[] { burrowStateSettings.phaseOneSettings, 
												burrowStateSettings.phaseTwoSettings, 
												burrowStateSettings.phaseThreeSettings, 
												burrowStateSettings.phaseFourSettings };

		StartBurrowSequence();
	}

	private void StartBurrowSequence()
	{
		//.. do animation stuff

		OnBurrowed(); // temp
	}

	private void StartUnburrowSequence()
	{
		// move the spider tank to its new position
		spiderTank.transform.position = GetNearestRelocationPosition();

		//.. do animation stuff

		OnUnburrowed(); // temp
	}

	private void OnBurrowed()
	{
		// start the unburrow sequence after x time underground
		Invoke( "StartUnburrowSequence", _settings[spiderTank.currentPhase].timeUnderground );
	}

	private void OnUnburrowed()
	{
		//... do stuff, if necessary
		// disable state??
	}

	private Vector3 GetRandomRelocationPosition()
	{
		return relocationPoints[Random.Range( 0, relocationPoints.Length )].position;
	}

	private Vector3 GetNearestRelocationPosition()
	{
		Vector3 nearest = relocationPoints[0].position;
		float nearestDistance = Vector3.Distance( nearest, player.transform.position );

		for ( int i = 1; i < relocationPoints.Length; i++ )
		{
			Vector3 pos = relocationPoints[i].position;
			float distance = Vector3.Distance( pos, player.transform.position );

			if ( distance < nearestDistance )
			{
				nearest = pos;
				nearestDistance = distance;
			}
		}

		return nearest;
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
