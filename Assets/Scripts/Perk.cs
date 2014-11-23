using UnityEngine;
using System.Collections;

public enum PerkType
{
	SpecialGun,
	HealthPack,
	SpeedBoost,
	Shield,
	WeaponSpeed,
	WeaponDamage
}

[System.Serializable]
public class PerkData
{
	public PerkType type;

	//bools to determine what is being modified by a perk object
	public bool shield;
	public bool infiniteAmmo;

	//ammount to change various stats by
	public float speedMod;
	public float fireRateMod;
	public float healthMod;
	public float damageMod;
	public float reloadMod;
	public int magazinesMod;
	public GameObject gunDrop;
	public float duration; //!< How long the perk lasts, if it has a duration.
}

public class Perk : MonoBehaviour
{
	[Range( 0.0f, 1f )]
	public float dropChance;
	public float maxPickupTime; //!< How long the perk object stays in the world before disappearing.

	public PerkData settings;

	void Awake()
	{
		Invoke( "Disappear", maxPickupTime );
	}

	void OnCollisionEnter( Collision other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			other.gameObject.GetComponent<PerkSystem>().AddPerk( this );

			CancelInvoke();
			Destroy( gameObject );
		}
	}

	void Disappear()
	{
		Destroy( gameObject );
	}
}
