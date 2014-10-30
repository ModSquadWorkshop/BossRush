using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState 
{
	public GameObject mortarLauncher;
	public int amountOfMortars;
	public float delayBeforeLaunch;

	private MortarAttack _mortarAttack;
	private Timer _mortarDelayTimer;

	public override void Awake()
	{
		base.Awake();

		_mortarDelayTimer = new Timer( delayBeforeLaunch, 1 );

		_mortarAttack = mortarLauncher.GetComponent<MortarAttack>();
		if ( _mortarAttack != null )
		{
			// auto assign the target of the mortar attack to be the player
			_mortarAttack.target = player.gameObject;

			// start the delay timer
			// when the delay is complete, the mortars start launching
			_mortarDelayTimer.Start();
		}
	}

	void Update()
	{
		if ( _mortarDelayTimer.running )
		{
			_mortarDelayTimer.Update();

			if ( _mortarDelayTimer.complete )
			{
				_mortarAttack.Launch( amountOfMortars );
			}
		}
	}
}
