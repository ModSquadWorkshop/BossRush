using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
	public GameObject[] weapons;
	public int defaultWeaponID = 0;
	public KeyCode switchWeaponKeybind;

	public Transform weaponAnchor; //< The location where the player's weapon should float.

	private int _currentWeaponID;
	private Weapon _currentWeapon;
	private Weapon _defaultWeapon;

	public const float JOYSTICK_THRESHOLD = 0.75f;

	void Start() 
	{
		/*
		// initialize the weapons
		*/

		for ( int i = 0; i < weapons.Length; ++i )
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

			Weapon weapon = ( Weapon )weaponObject.AddComponent( typeof( Weapon ) );
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
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		if ( Input.GetButton( "Fire1" ) || gamePadLook.sqrMagnitude > JOYSTICK_THRESHOLD )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeapon.IsOnCooldown )
			{
				return;
			}

			_currentWeapon.PerformPrimaryAttack();
		}

		// secondary weapon attack
		if ( Input.GetButton( "Fire2" ) )
		{
			// if the weapon is still on cooldown, it cannot perform an attack
			if ( _currentWeapon.IsOnCooldown )
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
		weapon.enabled = false;
		weapon.gameObject.SetActive( false );
		weapon.gameObject.transform.parent = weaponAnchor;
		weapon.gameObject.transform.localPosition = Vector3.zero;

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
		return ( Weapon )weapons[weaponID].GetComponent( typeof( Weapon ) );
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
		get
		{
			return _currentWeapon;
		}
	}
}