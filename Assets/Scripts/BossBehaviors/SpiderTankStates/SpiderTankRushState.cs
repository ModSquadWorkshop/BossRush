﻿using UnityEngine;
using System.Collections;

public class SpiderTankRushState : SpiderTankState
{
	[HideInInspector] public SpiderTankState returnState;

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
		returnState.enabled = true;
		_damageSystem.enabled = false;
	}
}
