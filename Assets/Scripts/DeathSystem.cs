using UnityEngine;
using System.Collections;

public class DeathSystem : MonoBehaviour
{
	public delegate void DeathCallback( GameObject gameObject );

	public AudioClip[] deathSounds;
	public bool allowMultipleDeaths;

	private DeathCallback _deathCallback = delegate( GameObject gameObject ) { };
	private bool _dying = false;

	public void RegisterDeathCallback( DeathCallback callback )
	{
		_deathCallback += callback;
	}

	public void DeregisterDeathCallback( DeathCallback callback )
	{
		_deathCallback -= callback;
	}

	public void NotifyDeath()
	{
		if ( deathSounds.Length > 0 )
		{
			audio.clip = deathSounds[Random.Range( 0, deathSounds.Length )];
			audio.volume = .9f;
			audio.priority = 0;
			audio.Play();
		}

		if ( !_dying || allowMultipleDeaths )
		{
			_dying = true;
			_deathCallback( gameObject );
		}
	}

	public void Kill()
	{
		NotifyDeath();
		Destroy( gameObject );
	}

	public void KillDelayed( float delay )
	{
		Invoke( "Kill", delay );
	}

	/**
	 * \brief Utility method to "kill" an object without actually killing it.
	 *
	 * \note Doesn't actually send out a death notification.
	 *
	 */
	public void Gut()
	{
		foreach ( MonoBehaviour script in GetComponents<MonoBehaviour>() )
		{
			Destroy( script );
		}
	}
}
