using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour 
{
	public float cooldown;
	protected Timer _cooldownTimer;

	void Update() 
	{
		_cooldownTimer.Update();
	}

	public void Init() 
	{
		_cooldownTimer = new Timer( cooldown, 1 );

		// the timer has to be started now because we need it to be in a "complete" state
		// until it is in a "complete" state, the gun won't fire since it is considered on cooldown
		_cooldownTimer.Start();
	}

	public virtual void PerformPrimaryAttack() 
	{
		// override
	}

	public virtual void PerformSecondaryAttack() 
	{
		// override
	}

	public bool IsOnCooldown() 
	{
		return !_cooldownTimer.IsComplete();
	}
}