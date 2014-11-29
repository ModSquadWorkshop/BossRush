using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeapons : MonoBehaviour
{
	public const float JOYSTICK_THRESHOLD = 0.75f;
	public const int SPECIAL_WEAPON_SLOT = 1;

	public List<GameObject> weapons;

	[HideInInspector] public PerkData perk;
	private PerkSystem _perkSystem;

	[Tooltip( "The location where the player's weapons should appear." )]
	public Transform weaponAnchor;
	public GameObject ammoRing;

	private int _currentWeaponIndex;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;
	private Gun _specialGun;
	private BeamWeapon _specialBeam;

	private float _fireRateMod;
	private float _damageMod;

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
		_defaultWeapon = GetWeapon( 0 );

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

		SwitchWeapon( 0 );

		_perkSystem = GetComponent<PerkSystem>();

		_fireRateMod = 0.0f;
		_damageMod = 0.0f;
	}

	void Update()
	{
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		if ( Input.GetButton( "Fire1" ) || gamePadLook.sqrMagnitude > JOYSTICK_THRESHOLD )
		{
			_currentWeapon.PerformPrimaryAttack();

			// check if the current weapon is out of ammo
			if ( _currentWeaponIndex == SPECIAL_WEAPON_SLOT )
			{
				if ( DetermineSpecialType() && _specialGun.isOutOfAmmo )
				{
					RemoveSpecialWeapon();
				}
				else if ( !DetermineSpecialType() && _specialBeam.isDone )
				{
					RemoveSpecialWeapon();
				}
			}
		}
	}

	void OnPause()
	{
		enabled = false;
	}

	void OnUnpause()
	{
		enabled = true;
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

	/**
	 * \brief Switches the active weapon to the specified one.
	 *
	 * \note No checking is done that \a weaponIndex is a valid value, so an exception will be thrown if it is not.
	 */
	public void SwitchWeapon( int weaponIndex )
	{
		// deactivate the previous weapon
		if ( _currentWeapon != null ) // TODO: Is there ever a case where _currentWeapon is null? I would think not.
		{
			_currentWeapon.enabled = false;
			_currentWeapon.gameObject.SetActive( false );
		}

		// equip the new weapon
		_currentWeapon = GetWeapon( weaponIndex );
		_currentWeaponIndex = weaponIndex;

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
		return NormalizeWeaponID( _currentWeaponIndex + 1 );
	}

	private int GetPreviousWeaponID()
	{
		return NormalizeWeaponID( _currentWeaponIndex - 1 );
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

		// disable possibly active and set to current gun
		ammoRing.SetActive( false );
		ammoRing.SetActive( true );
	}

	public void RemoveSpecialWeapon()
	{
		ammoRing.SetActive( false );

		Destroy( GetWeapon( SPECIAL_WEAPON_SLOT ).gameObject );
		weapons.RemoveAt( SPECIAL_WEAPON_SLOT );

		_perkSystem.RemovePerk( perk );

		SwitchWeapon( 0 );
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
			Gun gun = weapon.GetComponent<Gun>();
			if ( gun != null )
			{
				gun.cooldown += fireRate;
			}
			weapon.GetComponent<DamageSystem>().damageMultiplier += damage;
		}

		CurrentBuffs( fireRate, damage, reloadSpeed );
	}

	public void SetBuffs()
	{
		//apply all current buffs to special weapon
		Gun gun = weapons[SPECIAL_WEAPON_SLOT].GetComponent<Gun>();
		if ( gun != null )
		{
			gun.cooldown += _fireRateMod;
		}
		weapons[SPECIAL_WEAPON_SLOT].GetComponent<DamageSystem>().damageMultiplier += _damageMod;
	}

	public void RevertBuff( float fireRate, float damage, float reloadSpeed )
	{
		//remove buff of 1 perk from all weapons in system
		foreach ( GameObject weapon in weapons )
		{
			Gun gun = weapon.GetComponent<Gun>();
			if ( gun != null )
			{
				gun.cooldown -= fireRate;
			}
			weapon.GetComponent<DamageSystem>().damageMultiplier -= damage;
		}

		CurrentBuffs( -fireRate, -damage, -reloadSpeed );
	}

	public void CurrentBuffs( float fireRate, float damage, float reloadSpeed )
	{
		//sets all currently applicable modifiers to proper values
		_fireRateMod += fireRate;
		_damageMod += damage;
	}
}
