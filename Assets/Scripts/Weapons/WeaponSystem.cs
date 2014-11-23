using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
	public List<GameObject> weapons;
	public PerkData perk;
	private PerkSystem _perkSystem;
	public int defaultWeaponID = 0;
	public KeyCode switchWeaponKeybind;

	public Transform weaponAnchor; //< The location where the player's weapon should float.

	private int _currentWeaponID;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;
	private Gun _specialGun;
	public GameObject _AmmoRing;
	private BeamWeapon _specialBeam;

	private float _rateMod;
	private float _damageMod;
	private float _reloadMod;

	public const float JOYSTICK_THRESHOLD = 0.75f;
	public const int SPECIAL_WEAPON_SLOT = 1;

	void Start()
	{
		/*
		// initialize the weapons
		*/

		for ( int i = 0; i < weapons.Count; ++i )
		{
			// re-assigning the GameObject is import because Instantiate() creates a clone
			// when switching weapons, we need to get the Weapon component of the correct object (the clone)
			weapons[i] = Instantiate( weapons[i] ) as GameObject;
			InitializeWeapon( GetWeapon( i ) );

			// the weapon prefabs don't default to hitting enemies (only scenery),
			// so add it to their list of targets.
			weapons[i].GetComponent<DamageSystem>().targets.Add( "Enemy" );
		}

		/*
		// set the default weapon
		*/

		_defaultWeapon = GetWeapon( defaultWeaponID );

		// we must insure the deafult weapon is not null
		// if it is, we create a blank weapon to prevent future errors
		if ( _defaultWeapon == null )
		{
			GameObject weaponObject = new GameObject();
			weaponObject.SetActive( false );

			Weapon weapon = weaponObject.AddComponent<Weapon>();
			_defaultWeapon = InitializeWeapon( weapon );
		}

		/*
		// set the current weapon
		*/

		SwitchWeapon( defaultWeaponID );

		_perkSystem = GetComponent<PerkSystem>();

		_rateMod = 0.0f;
		_damageMod = 0.0f;
		_reloadMod = 0.0f; 
	}

	void Update()
	{
		/*
		if ( Input.GetButtonDown( "Switch" ) )
		{
			NextWeapon();
		}
		 * */

		/*
		// primary weapon attack
		*/

		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		if ( Input.GetButton( "Fire1" ) || gamePadLook.sqrMagnitude > JOYSTICK_THRESHOLD )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeaponID == SPECIAL_WEAPON_SLOT )
			{
				if ( DetermineSpecialType() && _specialGun.IsOutOfAmmo() )
				{
					RemoveSpecialWeapon();
				}
				else if ( !DetermineSpecialType() && _specialBeam.IsDone() )
				{
					RemoveSpecialWeapon();
				}
			}
			if ( !_currentWeapon.isOnCooldown )
			{
				_currentWeapon.PerformPrimaryAttack();
			}
		}


		/*
		// secondary weapon attack
		*/

		/*
		if ( Input.GetButton( "Fire2" ) )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( !_currentWeapon.isOnCooldown )
			{
				_currentWeapon.PerformSecondaryAttack();
			}
		}
		 * */
	}

	private Weapon InitializeWeapon( Weapon weapon )
	{
		weapon.enabled = false;
		weapon.gameObject.SetActive( false );
		weapon.gameObject.transform.parent = weaponAnchor;
		weapon.gameObject.transform.localPosition = Vector3.zero;
		weapon.gameObject.transform.localRotation = Quaternion.identity;

		return weapon;
	}

	public void SwitchWeapon( int weaponID )
	{
		// deactivate the previous weapon
		if ( _currentWeapon != null )
		{
			_currentWeapon.enabled = false;
			_currentWeapon.gameObject.SetActive( false );
		}

		// equip the new weapon
		_currentWeapon = GetWeapon( weaponID );
		_currentWeaponID = weaponID;

		// if an invalid weaponID is passed,
		// the currentWeapon will be set to the deaultWeapon to prevent errors
		if ( _currentWeapon == null )
		{
			_currentWeapon = _defaultWeapon;
			_currentWeaponID = defaultWeaponID;
		}

		// activate the new weapon
		_currentWeapon.enabled = true;
		_currentWeapon.gameObject.SetActive( true );
	}

	public void NextWeapon()
	{
		SwitchWeapon( GetNextWeaponID() );
	}

	public void PreviousWeapon()
	{
		SwitchWeapon( GetPreviousWeaponID() );
	}

	public Weapon GetWeapon( int weaponID )
	{
		return weapons[weaponID].GetComponent<Weapon>();
	}

	private int GetNextWeaponID()
	{
		return NormalizeWeaponID( _currentWeaponID + 1 );
	}

	private int GetPreviousWeaponID()
	{
		return NormalizeWeaponID( _currentWeaponID - 1 );
	}

	private int NormalizeWeaponID( int weaponID )
	{
		if ( weaponID >= weapons.Count )
		{
			weaponID = 0;
		}

		if ( weaponID < 0 )
		{
			weaponID = weapons.Count - 1;
		}

		return weaponID;
	}

	public Weapon currentWeapon
	{
		get
		{
			return _currentWeapon;
		}
	}

	public void NewSpecialWeapon()
	{
		// initialize special slot weapon
		weapons[SPECIAL_WEAPON_SLOT] = Instantiate( weapons[SPECIAL_WEAPON_SLOT] ) as GameObject;
		InitializeWeapon( GetWeapon( SPECIAL_WEAPON_SLOT ) );

		// the weapon prefabs don't default to hitting enemies (only scenery),
		// so add it to their list of targets.
		weapons[SPECIAL_WEAPON_SLOT].GetComponent<DamageSystem>().targets.Add( "Enemy" );
		SwitchWeapon( SPECIAL_WEAPON_SLOT );

		_specialGun = weapons[SPECIAL_WEAPON_SLOT].GetComponent<Gun>();
		_specialBeam = weapons[SPECIAL_WEAPON_SLOT].GetComponent<BeamWeapon>();

		SetBuffs();

		//disable possibly active and set to current gun
		_AmmoRing.gameObject.SetActive( false );
		_AmmoRing.gameObject.SetActive( true );
	}

	public void RemoveSpecialWeapon()
	{
		_AmmoRing.SetActive( false );

		Destroy( GetWeapon( SPECIAL_WEAPON_SLOT ).gameObject );
		weapons.RemoveAt( SPECIAL_WEAPON_SLOT );

		_perkSystem.RemovePerk( perk );

		SwitchWeapon( defaultWeaponID );
	}

	public bool DetermineSpecialType()
	{
		//return true if gun, false if beam
		return _specialGun != null;
	}

	public void SetBuff( float fireRate, float damage, float reloadSpeed )
	{
		//apply buff of 1 perk to all weapons
		foreach ( GameObject weapon in weapons )
		{
			weapon.GetComponent<Weapon>().cooldown += fireRate;
			weapon.GetComponent<DamageSystem>().damageMultiplier += damage;

			if ( weapon.GetComponent<Gun>() != null )
			{
				weapon.GetComponent<Gun>().reloadSpeed += reloadSpeed;
			}
		}

		CurrentBuffs( fireRate, damage, reloadSpeed );
	}

	public void SetBuffs()
	{
		//apply all current buffs to special weapon
		weapons[SPECIAL_WEAPON_SLOT].GetComponent<Weapon>().cooldown += _rateMod;
		weapons[SPECIAL_WEAPON_SLOT].GetComponent<DamageSystem>().damageMultiplier += _damageMod;

		if ( DetermineSpecialType() )
		{
			weapons[SPECIAL_WEAPON_SLOT].GetComponent<Gun>().reloadSpeed += _reloadMod;
		}
	}

	public void RevertBuff( float fireRate, float damage, float reloadSpeed )
	{
		//remove buff of 1 perk from all weapons in system
		foreach ( GameObject weapon in weapons )
		{
			weapon.GetComponent<Weapon>().cooldown -= fireRate;
			weapon.GetComponent<DamageSystem>().damageMultiplier -= damage;

			if ( weapon.GetComponent<Gun>() != null )
			{
				weapon.GetComponent<Gun>().reloadSpeed -= reloadSpeed;
			}
		}

		CurrentBuffs( -fireRate, -damage, -reloadSpeed );
	}

	public void CurrentBuffs( float fireRate, float damage, float reloadSpeed )
	{
		//sets all currently applicable modifiers to proper values
		_rateMod += fireRate;
		_damageMod += damage;
		_reloadMod += reloadSpeed;
	}
}