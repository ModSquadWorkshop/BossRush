using UnityEngine;
using System.Collections;

public class PerkSystem : MonoBehaviour 
{
	//bools to determine what is being modified by a perk object
	public bool immunity;

	//ammount to change various stats by
	public float speedMod;
	public float fireRateMod; //make a negative value
	public float maxHealthMod;
	public float damageMod;

	//time a perk lasts (if using a timer);
	public float length;
	
	void OnCollisionEnter( Collision other )
	{
		//Debug.Log ( "HIT" );
		if( other.gameObject.tag == "Player" )
		{
			other.gameObject.GetComponent<PlayerMovement>().speedMultiplier += speedMod;

			other.gameObject.GetComponent<HealthSystem>().maxHealth += maxHealthMod; //heal to new max once picked up?

			//only effect currently held gun right now
			other.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>().damageMultiplier += damageMod;
			other.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().cooldown += fireRateMod;

			if( immunity )
			{
				other.gameObject.GetComponent<PerkTime>().perkLength = length;
				other.gameObject.GetComponent<PerkTime>().BeginPerk();
				other.gameObject.GetComponent<HealthSystem>().immune = true;
			}
			Destroy(this.gameObject);
		}
	}
}
/*
Questions for designers
_______________________
Should a max health boost also heal the player?
Should a fire rate/ammo/damage boost effect all guns held or just the one currently held? (GetComponentInChildren)
*/
