using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour 
{
	public GameObject[] weapons;
	public int defaultWeaponID = 0;
	public KeyCode switchWeaponKeybind;

	private int _currentWeaponID;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;

	void Start() 
	{
		/* 
		// initialize the weapons 
		*/

		for ( int i = 0; i < weapons.Length; ++i ) 
		{
			// re-assigning the GameObject is import because Instantiate() creates a clone
			// when switching weapons, we need to get the Weapon component of the correct object (the clone)
			weapons[i] = (GameObject)Instantiate( weapons[i] );
			InitializeWeapon( GetWeapon( i ) );
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

			Weapon weapon = (Weapon)weaponObject.AddComponent( typeof( Weapon ) );
			_defaultWeapon = InitializeWeapon( weapon );
		}
		
		/*
		// set the current weapon
		*/
		
		SwitchWeapon( defaultWeaponID );
	}

	void Update() 
	{
		// primary weapon attack
		if ( Input.GetButton( "Fire1" ) ) 
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeapon.IsOnCooldown() ) 
			{
				return;
			}

			_currentWeapon.PerformPrimaryAttack();
		}
		
		// secondary weapon attack
		if ( Input.GetButton( "Fire2" ) ) 
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeapon.IsOnCooldown() ) 
			{
				return;
			}

			_currentWeapon.PerformSecondaryAttack();
		}

		if ( Input.GetKeyDown( switchWeaponKeybind ) ) 
		{
			NextWeapon();
		}
	}

	private Weapon InitializeWeapon( Weapon weapon ) 
	{
		weapon.Init();
		weapon.enabled = false;
		weapon.gameObject.SetActive( false );
		weapon.gameObject.transform.parent = this.transform;

		// when adding the weapon as a child of this object, the scaling gets messed up. 
		// the following code fixes it by re-multiplying the parent scale to the weapons scale and position
		Vector3 scale = this.transform.localScale;
		Vector3 v = weapon.gameObject.transform.localScale;
		weapon.gameObject.transform.localScale = new Vector3( scale.x * v.x, scale.y * v.y, scale.z * v.z );
		v = weapon.gameObject.transform.position;
		weapon.gameObject.transform.position = new Vector3( scale.x * v.x, scale.y * v.y, scale.z * v.z );
		
		return weapon;
	}

	public void SwitchWeapon( int weaponID ) 
	{
		// deactivate the previous weapon
		if (_currentWeapon != null) 
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
		return (Weapon)weapons[weaponID].GetComponent( typeof( Weapon ) );
	}

	private int GetNextWeaponID() 
	{
		return NormalizeWeaponID(_currentWeaponID + 1);
	}

	private int GetPreviousWeaponID() 
	{
		return NormalizeWeaponID(_currentWeaponID - 1);
	}

	private int NormalizeWeaponID( int weaponID ) 
	{
		if ( weaponID >= weapons.Length ) 
		{
			weaponID = 0;
		}

		if ( weaponID < 0 ) 
		{
			weaponID = weapons.Length - 1;
		}

		return weaponID;
	}

	public Weapon currentWeapon 
	{
		get { return _currentWeapon; }
	}
}