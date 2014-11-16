using UnityEngine;
using System.Collections;

public class SpiderTankBurrowState : SpiderTankState 
{
	[HideInInspector] public SpiderTankState returnState;

	public BurrowStateSettingsList burrowStateSettings;
	private BurrowStateSettings[] _settings;

	public Transform[] relocationPoints;

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
		DoBurrowAnimation();
		Invoke( "OnBurrowed", _settings[spiderTank.currentPhase].burrowSequenceDuration );
	}

	private void DoBurrowAnimation()
	{
		//... do animation stuff
	}

	private void OnBurrowed()
	{
		StopRendering();
		Invoke( "StartUnburrowSequence", _settings[spiderTank.currentPhase].timeUnderground );
	}

	private void StartUnburrowSequence()
	{
		// move the spider tank to its new position
		spiderTank.transform.position = GetNearestRelocationPosition();

		StartRendering();
		DoUnburrowAnimation();
		Invoke( "OnUnburrowed", _settings[spiderTank.currentPhase].burrowSequenceDuration );
	}

	private void DoUnburrowAnimation()
	{
		//... do animation stuff
	}

	private void OnUnburrowed()
	{
		enabled = false;
		returnState.enabled = true;
	}

	private void StartRendering()
	{
		for ( int i = 0; i < spiderTank.meshes.Length; i++ )
		{
			spiderTank.meshes[i].enabled = true;
		}

		spiderTank.boxCollider.enabled = true;
		spiderTank.ringUICanvas.enabled = true;
	}

	private void StopRendering()
	{
		for ( int i = 0; i < spiderTank.meshes.Length; i++ )
		{
			spiderTank.meshes[i].enabled = false;
		}

		spiderTank.boxCollider.enabled = false;
		spiderTank.ringUICanvas.enabled = false;
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
	public float burrowSequenceDuration;
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
