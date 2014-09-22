using UnityEngine;
using System.Collections;

sealed public class HealthSystem : MonoBehaviour
{
	public bool immune = false;
	public bool destroyOnNoLives = true;

	public int startingLives = 1;
	public int maxLives = 1;

	public float startingHealth = 100.0f;
	public float maxHealth = 100.0f;

	[SerializeField] private int _lives;
	[SerializeField] private float _health;

	void Start()
	{
		_health = Mathf.Clamp( startingHealth, 0.0f, maxHealth );
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

		if ( _lives <= 0 )
		{
			if ( destroyOnNoLives )
			{
				Destroy( this.gameObject );
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
