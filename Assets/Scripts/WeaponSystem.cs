using UnityEngine;
using System.Collections;

sealed public class WeaponSystem : MonoBehaviour 
{
	public string[] weaponsAvailable;
	public int defaultWeaponID = 0;

	private Hashtable _weapons;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;

	void Start()
	{
		/*
		// initialize the weapons
		*/

		// in case the MasterWeaponsList has yet to be initialized, we do it here
		MasterWeaponsList.Init();

		_weapons = new Hashtable();

		// the list of weapons this object has available should be provided before initialization
		for ( int i = 0; i < weaponsAvailable.Length; i++ ) 
		{
			_weapons[i] = MasterWeaponsList.CloneWeapon( weaponsAvailable[i], this.gameObject );
		}


		/*
		// set the default weapon
		*/

		_defaultWeapon = (Weapon)_weapons[defaultWeaponID];

		// we must insure the deafult weapon is not null
		// if it is, we instantiate a blank Weapon to prevent future errors
		if ( _defaultWeapon == null ) 
		{
			_defaultWeapon = new Weapon();
		}


		/*
		// set the current weapon
		*/
		
		SwitchWeapon( defaultWeaponID );
	}

	void Update () 
	{
		// updating the weapon is necessary to advance its cooldown
		_currentWeapon.Update();

		// primary weapon attack
		if ( Input.GetButton( "Fire1" ) ) 
		{
			_currentWeapon.PerformPrimaryAttack();
		}

		// secondary weapon attack
		if ( Input.GetButton( "Fire2" ) ) 
		{
			_currentWeapon.PerformSecondaryAttack();
		}
	}

	public void SwitchWeapon ( int weaponID ) 
	{
		// equip the weapon
		_currentWeapon = (Weapon)_weapons[weaponID];

		// if an invalid weaponID is passed, 
		// the currentWeapon will be set to the deaultWeapon to prevent errors
		if ( _currentWeapon == null ) 
		{
			_currentWeapon = _defaultWeapon;
		}
	}

	public Weapon currentWeapon 
	{
		get { return _currentWeapon; }
		//set { _currentWeapon = value; }
	}

	public float damage 
	{
		get { return _currentWeapon.damage; }
	}
}
