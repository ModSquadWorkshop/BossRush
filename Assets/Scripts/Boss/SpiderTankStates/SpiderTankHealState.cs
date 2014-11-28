using UnityEngine;
using System.Collections;

public class SpiderTankHealState : SpiderTankState
{
	public HealStateSettingsList healStateSettings;
	private HealStateSettings[] _settings;
	public AudioClip BossHeal;

	public void Update()
	{
		spiderTank.health.Heal( _settings[spiderTank.currentPhase].healRate * Time.deltaTime );

		if ( spiderTank.atMaxHealth )
		{
			ShieldDestroyed( shield ); // make sure we go through a common exit path
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new HealStateSettings[] { healStateSettings.phaseOneSettings, 
											  healStateSettings.phaseTwoSettings, 
											  healStateSettings.phaseThreeSettings, 
											  healStateSettings.phaseFourSettings };

		shield.SetActive( true );
		shield.gameObject.GetComponent<DeathSystem>().RegisterDeathCallback( ShieldDestroyed );

		Physics.IgnoreCollision( collider, shield.collider, true );
		audio.clip = BossHeal;
		audio.volume = .8f;
		audio.priority = 0;
		audio.Play();
		audio.loop = true;
	}

	public override void OnDisable()
	{
		base.OnDisable();

		spiderTank.SetDamageBase();

		// on shutdown the shield gets destroyed before the spider tank,
		// so we have the potential for null references here
		if ( shield != null )
		{
			shield.SetActive( false );
			shield.gameObject.GetComponent<DeathSystem>().DeregisterDeathCallback( ShieldDestroyed );
		}
	}

	public void ShieldDestroyed( GameObject shield )
	{
		enabled = false;
		spiderTank.basicState.enabled = true;
	}
}


[System.Serializable]
public class HealStateSettings
{
	public float healRate;
}


// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class HealStateSettingsList
{
	public HealStateSettings phaseOneSettings;
	public HealStateSettings phaseTwoSettings;
	public HealStateSettings phaseThreeSettings;
	public HealStateSettings phaseFourSettings;
}
