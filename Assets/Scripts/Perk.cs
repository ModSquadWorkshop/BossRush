using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour
{
	[Range( 0.0f, 1f )]
	public float dropChance;

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
	public string ID;
	//time a perk lasts (if using a timer);
	public float duration;

	PerkSystem perkSystem;

	void OnCollisionEnter( Collision other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			perkSystem = other.gameObject.GetComponent<PerkSystem>();
			perkSystem.AddPerk( this );

			gameObject.SetActive( false );
		}
	}

	public void Refresh()
	{
		CancelInvoke();
		Begin();
	}

	public void Begin()
	{
		if ( duration > 0 )
		{
			Invoke( "End", duration );
		}
		else
		{
			Destroy( this.gameObject );
		}
	}

	void End()
	{
		perkSystem.RemovePerk( this );
		Destroy( this.gameObject );
	}
}