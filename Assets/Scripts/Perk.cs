using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour
{
	public DropSettings settings;

	//bools to determine what is being modified by a perk object
	public bool immunity;
	public bool infiniteAmmo;

	//ammount to change various stats by
	public float speedMod;
	public float fireRateMod;
	public float maxHealthMod;
	public float healthMod;
	public float damageMod;
	public float reloadMod;
	public int magazinesMod;
	public GameObject gunDrop;
	//time a perk lasts (if using a timer);
	public float length;

	void OnCollisionEnter( Collision other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			other.gameObject.GetComponent<PerkSystem>().AddPerk( this );
			Destroy( this.gameObject );
		}
	}
}

[System.Serializable]
public class DropSettings
{
	[Range( 0.0f, 1f )]
	public float minChance;
	[Range( 0.0f, 1f )]
	public float maxChance;
}