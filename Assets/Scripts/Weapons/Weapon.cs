using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float cooldown;

	private bool _cooling;

	public virtual void PerformPrimaryAttack() { }

	public virtual void PerformSecondaryAttack() { }

	public void PlayPrimarySound()
	{
		audio.Play();
	}

	public void StartCooldown()
	{
		_cooling = true;
		Invoke( "EndCooldown", cooldown );
	}

	public void EndCooldown()
	{
		_cooling = false;
	}

	public void SetCooldown( float newCooldown )
	{
		cooldown = newCooldown;
	}

	public bool isOnCooldown
	{
		get
		{
			return _cooling;
		}
	}
}
