using UnityEngine;
using System.Collections;

public class PlayerShield : MonoBehaviour
{
	public Color fullHealthColor;
	public Color noHealthColor;

	private HealthSystem _health;
	private DeathSystem _death;
	private Renderer _renderer;
	private PerkSystem _perks;
	private Perk _perk;

	void Awake()
	{
		_renderer = renderer;
		_health = GetComponent<HealthSystem>();
		_death = GetComponent<DeathSystem>();
		_perks = GetComponentInParent<PerkSystem>();
		_health.RegisterHealthCallback( HealthChangeCallback );
		_death.RegisterDeathCallback( DeathCallback );
	}

	void OnEnable()
	{
		_health.Reset();
		_renderer.material.color = fullHealthColor;
	}

	void HealthChangeCallback( HealthSystem health, float change )
	{
		if ( _health.alive )
		{
			_renderer.material.color = Color.Lerp( noHealthColor, fullHealthColor, _health.percent );
		}
	}

	void DeathCallback( GameObject shield )
	{
		_perks.RemovePerk( _perk );
	}

	float RemainingHealth()
	{
		return _health.health;
	}

	public void RestoreShield()
	{
		_health.health = _health.maxHealth;
	}

	public void SetPerk( Perk perk )
	{
		_perk = perk;
	}
 
}
