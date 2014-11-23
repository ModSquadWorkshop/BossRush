using UnityEngine;
using System.Collections;

public class DeathSystem : MonoBehaviour
{
	public delegate void DeathCallback( GameObject gameObject );

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
