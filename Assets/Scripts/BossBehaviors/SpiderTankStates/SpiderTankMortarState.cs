using UnityEngine;
using System.Collections;

public class SpiderTankMortarState : SpiderTankState 
{
	public GameObject mortarLauncher;
	public int amountOfMortars;
	public float delayBeforeLaunch;

	private MortarAttack _mortarAttack;

	public override void Awake()
	{
		base.Awake();

		_mortarAttack = mortarLauncher.GetComponent<MortarAttack>();
		if ( _mortarAttack != null )
		{
			// auto assign the target of the mortar attack to be the player
			_mortarAttack.target = player.gameObject;
		}

		Invoke( "Launch", delayBeforeLaunch );
	}

	void Launch()
	{
		_mortarAttack.Launch( amountOfMortars );
	}
}
