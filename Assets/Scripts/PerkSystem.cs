using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerkSystem : MonoBehaviour
{
	public Perk[] startingPerks;
	public Shield shield;

	private Dictionary<PerkType, PerkData> _perks;
	private Dictionary<PerkType, Coroutine> _perkEnding;
	private PlayerMovement _playerSpeed;
	private HealthSystem _playerHealth;
	private WeaponSystem _playerWeapons;

	void Awake()
	{
		_perks = new Dictionary<PerkType, PerkData>();
		_perkEnding = new Dictionary<PerkType, Coroutine>();
		for ( int i = 0; i < startingPerks.Length; i++ )
		{
			AddPerk( startingPerks[i] );
		}

		_playerSpeed = GetComponent<PlayerMovement>();
		_playerHealth = GetComponent<HealthSystem>();
		_playerWeapons = GetComponent<WeaponSystem>();

		shield.GetComponent<DeathSystem>().RegisterDeathCallback( PlayerShieldDestroyed );
	}

	public void AddPerk( Perk perk )
	{
		PerkData settings = perk.settings;

		// stash perk settings
		_perks[settings.type] = settings;

		// apply modifiers
		if ( settings.speedMod > 0.0f )
		{
			_playerSpeed.speedMultiplier = settings.speedMod;
		}

		_playerHealth.Heal( settings.healthMod );
		_playerWeapons.SetBuff( settings.fireRateMod, settings.damageMod, settings.reloadMod );

		if ( settings.shield )
		{
			_playerHealth.immune = true;
			shield.gameObject.SetActive( true );
			shield.ResetShield();
		}

		if ( settings.gunDrop != null && _playerWeapons.weapons.Count <= 3 )
		{
			if ( _playerWeapons.weapons.Count == 3 )
			{
				_playerWeapons.RemoveSpecial();
			}
			_playerWeapons.weapons.Add( settings.gunDrop );
			_playerWeapons.perk = settings;
			_playerWeapons.NewWeapon();
		}

		if ( settings.duration > 0.0f )
		{
			// cancel existing coroutine
			if ( _perkEnding.ContainsKey( settings.type ) )
			{
				//StopCoroutine( _perkEnding[settings.type] ); // This is causing unity (the editor) to crash. It's a bug within unity, we'll have to work aroun it.
			}

			// start new coroutine
			_perkEnding[settings.type] = StartCoroutine( RemoveAfterDelay( settings ) );
		}
	}

	public IEnumerator RemoveAfterDelay( PerkData settings )
	{
		yield return new WaitForSeconds( settings.duration );
		RemovePerk( settings );
	}

	public void RemovePerk( PerkData perk )
	{
		//revert modifiers
		if ( perk.speedMod > 0.0f )
		{
			_playerSpeed.speedMultiplier = 1.0f;
		}

		_playerWeapons.RevertBuff( perk.fireRateMod, perk.damageMod, perk.reloadMod );

		_perks.Remove( perk.type );
		_perkEnding.Remove( perk.type );
	}

	void PlayerShieldDestroyed( GameObject shieldObject )
	{
		shield.gameObject.SetActive( false );
		_playerHealth.immune = false;
	}
}
