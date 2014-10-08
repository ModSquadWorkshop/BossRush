using UnityEngine;
using System.Collections;

sealed public class HealthSystem : MonoBehaviour
{
	public delegate void DeathCallback( HealthSystem self );

	public bool immune = false;
	public bool destroyOnNoLives = true;

	public int startingLives = 1;
	public int maxLives = 1;

	public float startingHealth = 100.0f;
	public float maxHealth = 100.0f;

	public AudioClip[] damageSounds;
	public AudioClip[] deathSounds;

	[SerializeField] private int _lives;
	[SerializeField] private float _health;

	private DeathCallback _deathCallback;

	void Start()
	{
		_health = Mathf.Clamp( startingHealth, 0.0f, maxHealth );
	}

	public void RegisterDeathCallback( DeathCallback callback )
	{
		// by the power of delegate composition,
		// I combine thee!
		_deathCallback += callback;
	}

	public float Damage( float n )
	{
		// if the object is immune, it cannot be damaged
		if ( immune )
		{
			return _health;
		}

		// if the damage amount is negative, its the same as healing the object
		if ( n < 0.0f )
		{
			return Heal( n );
		}

		if ( damageSounds.Length > 0 )
		{
			audio.PlayOneShot( damageSounds[Random.Range( 0, damageSounds.Length )] );
		}

		_health -= n;

		if ( _health < 0.0f )
		{
			Kill();
		}

		return _health;
	}

	public float Heal( float n )
	{
		// if the heal amount is negative, its the same as damaging the object
		if ( n < 0.0f )
		{
			return Damage( n );
		}

		_health = Mathf.Clamp( _health + n, 0.0f, maxHealth );

		return _health;
	}

	public void Kill()
	{
		_lives--;

		if ( deathSounds.Length > 0 )
		{
			audio.PlayOneShot( deathSounds[Random.Range( 0, deathSounds.Length )] );
		}

		if ( _lives <= 0 )
		{
			// call back listeners
			if ( _deathCallback != null )
			{
				_deathCallback( this );
			}

			if ( destroyOnNoLives )
			{
				Destroy( gameObject );
			}
			else
			{
				// clear death callbacks to prevent them
				// from being called twice.
				_deathCallback = null;
			}
		}
	}

	public int lives
	{
		get
		{
			return _lives;
		}
		set
		{
			_lives = Mathf.Clamp( value, 0, maxLives );
		}
	}

	public float health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = Mathf.Clamp( value, 0.0f, maxHealth );
		}
	}

	public float GetHealthPercent()
	{
		return ( _health / maxHealth ) * 100.0f;
	}

	public string GetHealthPercentAsString()
	{
		return GetHealthPercent().ToString() + "%";
	}

	public string GetHealthRatioAsString()
	{
		return _health.ToString() + " / " + maxHealth.ToString();
	}
}
