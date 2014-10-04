using UnityEngine;
using System.Collections;

sealed public class HealthSystem : MonoBehaviour
{
	public delegate void DeathCallback( HealthSystem self );
	public delegate void DamageCallback( HealthSystem self, float damage );

	public bool immune = false;
	public bool destroyOnNoLives = true;

	public int startingLives = 1;
	public int maxLives = 1;

	public float startingHealth = 100.0f;
	public float maxHealth = 100.0f;

	[SerializeField] private int _lives;
	[SerializeField] private float _health;

	private DeathCallback _deathCallback;
	private DamageCallback _damageCallback;

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

	public void RegisterDamageCallback( DamageCallback callback )
	{
		_damageCallback += callback;
	}

	public float Damage( float damage )
	{
		// if the object is immune, it cannot be damaged
		if ( immune )
		{
			return _health;
		}

		// if the damage amount is negative, its the same as healing the object
		if ( damage < 0.0f )
		{
			return Heal( damage );
		}

		_health -= damage;

		_damageCallback( this, damage );

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
