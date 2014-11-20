using UnityEngine;
using System.Collections;

public class SpiderTankRushState : SpiderTankState
{
	[HideInInspector] public SpiderTankState returnState;

	public AudioClip bossDashing;
	public RushAttackSettingsList rushStateSettings;
	private RushAttackSettings[] _settings;
	
	private RushAttack _rushAttack;
	private DamageSystem _damageSystem;

	public override void Awake()
	{
		base.Awake();

		_rushAttack = GetComponent<RushAttack>();
		_damageSystem = GetComponent<DamageSystem>();
	}

	public override void OnEnable()
	{
		base.OnEnable();

		_settings = new RushAttackSettings[] { rushStateSettings.phaseOneSettings, 
											   rushStateSettings.phaseTwoSettings, 
											   rushStateSettings.phaseThreeSettings, 
											   rushStateSettings.phaseFourSettings };

		_rushAttack.target = spiderTank.player;
		_rushAttack.RegisterCallback( new RushAttack.RushEndCallback( EndRush ) );
		_rushAttack.settings = _settings[spiderTank.currentPhase];
		_rushAttack.enabled = true;
		audio.clip = bossDashing;
		audio.Play();
		_damageSystem.enabled = true;
	}

	void EndRush()
	{
		enabled = false;
		_damageSystem.enabled = false;
		returnState.enabled = true;
	}
}


[System.Serializable]
public class RushAttackSettingsList
{
	public RushAttackSettings phaseOneSettings;
	public RushAttackSettings phaseTwoSettings;
	public RushAttackSettings phaseThreeSettings;
	public RushAttackSettings phaseFourSettings;
}