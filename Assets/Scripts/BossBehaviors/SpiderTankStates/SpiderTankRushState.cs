using UnityEngine;
using System.Collections;

public class SpiderTankRushState : SpiderTankState
{
	[HideInInspector] public SpiderTankState returnState;

	private RushAttack _rushAttack;

	public override void Awake()
	{
		base.Awake();

		_rushAttack = GetComponent<RushAttack>();
	}

	public void OnEnable()
	{
		_rushAttack.target = spiderTank.player;
		_rushAttack.RegisterCallback( new RushAttack.RushEndCallback( EndRush ) );
		_rushAttack.enabled = true;
	}

	void EndRush()
	{
		enabled = false;
		returnState.enabled = true;
	}
}
