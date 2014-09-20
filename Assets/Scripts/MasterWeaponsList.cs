using UnityEngine;
using System.Collections;

sealed public class MasterWeaponsList 
{
	public static Hashtable weapons = null;
	
	public static void Init () 
	{
		// the Master Weapons List only needs to be initialized once
		// if it is already created, then return
		if ( weapons != null ) 
		{
			return;
		}

		weapons = new Hashtable();

		Weapon weapon;

		weapon = Weapon.Instantiate();
		weapons[Weapon.NAME] = weapon;

		weapon = Gun.Instantiate();
		weapons[Gun.NAME] = weapon;

		Debug.Log( "initialized all weapons from the MasterWeaponList" );
	}

	public static Weapon GetWeapon( string weaponID ) 
	{
		return (Weapon)weapons[weaponID];
	}

	public static Weapon CloneWeapon ( string weaponID, GameObject parent = null ) 
	{
		Weapon weapon = GetWeapon( weaponID ).Clone();

		// setting the parent is optional
		if ( parent != null ) 
		{
			weapon.parent = parent;
		}

		return weapon;
	}
}