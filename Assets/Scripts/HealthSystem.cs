using UnityEngine;
using System.Collections;
using System.Diagnostics;

sealed public class HealthSystem : MonoBehaviour
{
	public delegate void HealthCallback( HealthSystem self, float change );

	public bool immune;
	public bool destroyOnDeath;

	public float startingHealth;
	public float maxHealth;

	public AudioClip[] damageSounds;

	private float _health;

	private HealthCallback _healthCallback = delegate( HealthSystem self, float change ) { };

	void Awake()
	{
		_health = Mathf.Clamp( startingHealth, 0.0f, maxHealth );
	}

	public void RegisterHealthCallback( HealthCallback callback )
	{
		_healthCallback += callback;
	}

	public float Damage( float damage )
	{
		System.Diagnostics.Debug.Assert( damage > 0.0f );

		// if the object is immune, it cannot be damaged
		if ( immune )
		{
			return _health;
		}

		if ( damageSounds.Length > 0 )
		{
			audio.PlayOneShot( damageSounds[Random.Range( 0, damageSounds.Length )] );
		}

		health -= damage;
		if ( _health <= 0.0f )
		{
			Kill();
		}

		return _health;
	}

	public float Heal( float healAmount )
	{
		System.Diagnostics.Debug.Assert( healAmount > 0.0f );

		health += healAmount;

		return health;
	}

	public void Kill()
	{
		if ( destroyOnDeath )
		{
			GetComponent<DeathSystem>().Kill();
		}
		else
		{
			GetComponent<DeathSystem>().NotifyDeath();
		}
	}

	/**
	 * \brief Resets health and lives to their starting values.
	 */
	public void Reset()
	{
		health = startingHealth;
	}

	public bool alive
	{
		get
		{
			return _health > 0.0f;
		}
	}

	/**
	 * \note This bypasses immunity and doesn't play any damage/healing sounds.
	 * If you actually mean to damage or heal the object, call Damage() or Heal().
	 */
	public float health
	{
		get
		{
			return _health;
		}

		set
		{
			float difference = value - _health;
			_health = Mathf.Clamp( value, 0.0f, maxHealth );
			_healthCallback( this, difference );
		}
	}

	public bool atMaxHealth
	{
		get
		{
			return _health >= maxHealth;
		}
	}

	public float percent
	{
		get
		{
			return _health / maxHealth;
		}
	}

	public string GetHealthPercentAsString()
	{
		return ( percent * 100.0f ).ToString() + "%";
	}

	public string GetHealthRatioAsString()
	{
		return _health.ToString() + " / " + maxHealth.ToString();
	}
}
