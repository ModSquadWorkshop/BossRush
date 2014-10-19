using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState 
{
	public GameObject mortarLauncher;
	public int mortarAmount;
	public float mortarDelay;

	private MortarAttack _mortarAttack;
	private Timer _mortarDelayTimer;

	public override void Awake()
	{
		base.Awake();

		_mortarDelayTimer = new Timer( mortarDelay, 1 );

		_mortarAttack = mortarLauncher.GetComponent<MortarAttack>();
		if ( _mortarAttack != null )
		{
			_mortarDelayTimer.Start();
		}
	}

	void Update()
	{
		if ( _mortarDelayTimer.IsRunning() )
		{
			_mortarDelayTimer.Update();

			if ( _mortarDelayTimer.IsComplete() )
			{
				_mortarAttack.Launch( mortarAmount );
			}
		}
	}

	public void onEnable()
	{
		//...
	}
}
