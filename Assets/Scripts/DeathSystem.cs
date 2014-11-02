using UnityEngine;
using System.Collections;

public class DeathSystem : MonoBehaviour
{
	public delegate void DeathCallback( GameObject gameObject );

	private DeathCallback _deathCallback = delegate( GameObject gameObject ) { };

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
		_deathCallback( gameObject );
	}

	public void Kill()
	{
		NotifyDeath();
		Destroy( gameObject );
	}

	public void Gut()
	{
		NotifyDeath();
		foreach ( MonoBehaviour script in GetComponents<MonoBehaviour>() )
		{
			Destroy( script );
		}
	}
}
