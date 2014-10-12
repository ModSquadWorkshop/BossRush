using UnityEngine;
using System.Collections;

public class SpiderTankRushState : SpiderTankState
{
	private RushAttack _rushAttack;
	private DamageSystem _damageSystem;

	public override void Awake()
	{
		base.Awake();

		_rushAttack = GetComponent<RushAttack>();
		_damageSystem = GetComponent<DamageSystem>();
	}

	public void OnEnable()
	{
		_rushAttack.target = spiderTank.player;
		_rushAttack.RegisterCallback( new RushAttack.RushEndCallback( EndRush ) );
		_rushAttack.enabled = true;
		_damageSystem.enabled = true;
	}

	void EndRush()
	{
		enabled = false;
		_damageSystem.enabled = false;

		spiderTank.EnterCurrentState();
	}
}
