using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float cooldown;

	public AudioClip[] primaryAttackSounds;
	private bool _cooling;

	public virtual void PerformPrimaryAttack() { }

	public virtual void PerformSecondaryAttack() { }

	public void PlayPrimarySound()
	{
		if ( primaryAttackSounds.Length > 0 && GetComponent<AudioSource>() != null )
		{
			audio.clip = primaryAttackSounds[Random.Range( 0, primaryAttackSounds.Length )];
			audio.Play();
		}
	}
	/*
	public void PlayLoopingSound()
	{
		if ( primaryAttackSounds.Length > 0 && GetComponent<AudioSource>() != null && !audio.isPlaying )
		{
			audio.clip = primaryAttackSounds[Random.Range( 0, primaryAttackSounds.Length )];
			audio.loop = true;
			audio.volume = 2;
			audio.Play();
		}
	}

	public void StopSound()
	{
		if ( GetComponent<AudioSource>() != null && audio.isPlaying )
		{
			audio.Stop();
		}
	}
	*/
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
