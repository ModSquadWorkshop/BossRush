using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float cooldown;
	protected Timer _cooldownTimer;

	public virtual void Start()
	{
		_cooldownTimer = new Timer( cooldown, 1 );

		// the timer has to be started now because we need it to be in a "complete" state
		// until it is in a "complete" state, the gun won't fire since it is considered on cooldown
		_cooldownTimer.Start();
	}

	void Update()
	{
		_cooldownTimer.Update();
	}

	public virtual void PerformPrimaryAttack() { }

	public virtual void PerformSecondaryAttack() { }

	public bool IsOnCooldown
	{
		get
		{
			return !_cooldownTimer.IsComplete();
		}
	}
}