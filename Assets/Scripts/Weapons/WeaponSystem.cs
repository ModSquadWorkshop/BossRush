using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
	public List<GameObject> weapons;
	public Perk perk;
	private PerkSystem _perkSystem;
	//public GameObject[] weapons;
	public int defaultWeaponID = 0;
	public KeyCode switchWeaponKeybind;

	public Transform weaponAnchor; //< The location where the player's weapon should float.

	private int _currentWeaponID;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;
	private Gun _specialGun;
	private BeamWeapon _specialBeam;
	public const float JOYSTICK_THRESHOLD = 0.75f;

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
	}

	public void NewWeapon()
	{
		//Initialize 3rd slot weapon
		weapons[2] = Instantiate( weapons[2] ) as GameObject;
		InitializeWeapon( GetWeapon( 2 ) );

		// the weapon prefabs don't default to hitting enemies (only scenery),
		// so add it to their list of targets.
		weapons[2].GetComponent<DamageSystem>().targets.Add( "Enemy" );
		SwitchWeapon( 2 );

		_specialGun = weapons[2].GetComponent<Gun>();
		_specialBeam = weapons[2].GetComponent<BeamWeapon>();
	}

	public bool DetermineType()
	{
		//return true if gun, false if beam
		return _specialGun != null;
	}

	void Update()
	{
		if ( Input.GetButtonDown( "Switch" ) )
		{
			NextWeapon();
		}

		/*
		// primary weapon attack
		*/

		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		if ( Input.GetButton( "Fire1" ) || gamePadLook.sqrMagnitude > JOYSTICK_THRESHOLD )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeaponID == 2 )
			{
				if ( DetermineType() && _specialGun.IsOutOfAmmo() )
				{
					RemoveSpecial();
				}
				else if ( !DetermineType() && _specialBeam.IsDone() )
				{
					RemoveSpecial();
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

		if ( Input.GetButton( "Fire2" ) )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( !_currentWeapon.isOnCooldown )
			{
				_currentWeapon.PerformSecondaryAttack();
			}
		}
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

	public void RemoveSpecial()
	{
		NextWeapon();
		Destroy( GetWeapon( 2 ).gameObject );
		weapons.RemoveAt( 2 );
		_perkSystem.RemovePerk( perk );
		
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

	public void SetBuffs( float fireRate, float damage, float reloadSpeed, bool infiniteAmmo )
	{
		//apply buffs to all weapons in system
	}

	public void RevertBuffs( float fireRate, float damage, float reloadSpeed, bool infiniteAmmo )
	{
		//remove buffs from all weapons in system
	}

	public void CurrentBuffs( float fireRate, float damage, float reloadSpeed, bool infiniteAmmo )
	{
		//keeps track of current buffs for newly added guns
	}
}