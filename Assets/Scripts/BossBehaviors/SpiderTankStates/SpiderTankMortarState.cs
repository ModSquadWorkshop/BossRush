using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState 
{
	public MortarAttack mortarAttack;
	public int mortarAmount;
	public float mortarDelay;

	private Timer _mortarDelayTimer;

	public override void Awake()
	{
		base.Awake();

		_mortarDelayTimer = new Timer( mortarDelay, 1 );
		_mortarDelayTimer.Start();
	}

	void Update()
	{
		if ( _mortarDelayTimer.IsRunning() )
		{
			_mortarDelayTimer.Update();

			if ( _mortarDelayTimer.IsComplete() )
			{
				mortarAttack.Launch( mortarAmount );
			}
		}
	}

	public void onEnable()
	{
		//...
	}
}
