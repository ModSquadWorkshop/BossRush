using UnityEngine;
using System.Collections;

public class Weapon 
{
	public const string NAME = "weapon";
	public const float COOLDOWN = 1.0f;
	public const float DAMAGE = 0.0f;

	public GameObject parent;
	public float damage;

	protected Timer _cooldownTimer;

	public Weapon () 
	{
		_cooldownTimer = new Timer( GetCooldownTime(), 1 );
		_cooldownTimer.Start();

		damage = GetDamage();

		Start();
	}

	public static Weapon Instantiate () 
	{
		// override
		return new Weapon();
	}

	public virtual Weapon Clone () 
	{
		// override
		Weapon weapon = new Weapon();
		weapon.parent = parent;
		weapon.damage = damage;
		return weapon;
	}

	public virtual void Update () 
	{
		// override, if necessary
		
		_cooldownTimer.Update();
	}

	protected virtual void Start () 
	{
		// override, if necessary
	}

	public virtual void PerformPrimaryAttack () 
	{
		// override
	}

	public virtual void PerformSecondaryAttack () 
	{
		// override
	}

	protected virtual float GetDamage () 
	{
		return DAMAGE;
	}

	protected virtual float GetCooldownTime () 
	{
		return COOLDOWN;
	}
}