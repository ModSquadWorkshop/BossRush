using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerkSystem : MonoBehaviour
{
	public Perk[] startingPerks;
	private Hashtable _perks;
	private Hashtable _perkCounts;

	PlayerMovement playerSpeed;
	HealthSystem playerHealth;
	DamageSystem playerDamage;
	Gun playerGun;
	WeaponSystem playerWeapons;

	void Start()
	{
		_perks = new Hashtable();
		_perkCounts = new Hashtable();
		for ( int i = 0; i < startingPerks.Length; i++ )
		{
			AddPerk( startingPerks[i] );
		}

		playerSpeed = this.gameObject.GetComponent<PlayerMovement>();
		playerHealth = this.gameObject.GetComponent<HealthSystem>();
		//playerDamage = this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>();
		//playerGun = this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>();
		playerWeapons = this.gameObject.GetComponent<WeaponSystem>();
	}

	public void AddPerk( Perk perk )
	{
		int actives = 0;
		if ( _perkCounts[perk.ID] != null )
		{
			actives = (int)_perkCounts[perk.ID];
		}
		else
		{
			_perkCounts[perk.ID] = 0;
		}

		if ( actives > 0 && ( perk.gunDrop != null || perk.duration > 0 ) )
		{
			Perk currentPerk = _perks[perk.ID] as Perk;
			if ( currentPerk != null )
			{
				currentPerk.Refresh();
			}
			Destroy( perk.gameObject );
		}
		else
		{
			_perks[perk.ID] = perk;

			int count = (int) _perkCounts[perk.ID];
			count++;
			_perkCounts[perk.ID] = count;

			SetPerk( perk );
			perk.Begin();
		}
	}

	public void RemovePerk( Perk perk )
	{
		ResetPerk( perk );

		int count = (int)_perkCounts[perk.ID];
		count--;
		_perkCounts[perk.ID] = count;

		_perks[perk.ID] = null;
	}

	public void RefreshPerk( Perk perk )
	{
		//refresh ammo if gun drop, otherwise refresh timer
		if ( perk.gunDrop != null )
		{
			Gun special = GetComponent<WeaponSystem>().weapons[2].GetComponent<Gun>();
			special.RefreshAmmo();
		}
		else if ( perk.duration > 0 )
		{
			perk.Refresh();
		}
	}

	public void SetPerk( Perk perk )
	{
		//apply modifiers
		playerSpeed.speedMultiplier += perk.speedMod;
		playerHealth.maxHealth += perk.maxHealthMod;
		playerHealth.Heal( perk.healthMod );
		//playerDamage.damageMultiplier += perk.damageMod;
		//playerGun.cooldown += perk.fireRateMod;
		//playerGun.amountOfMagazines += perk.magazinesMod;
		//playerGun.reloadSpeed += perk.reloadMod;
		//playerGun.infiniteAmmo = perk.infiniteAmmo || playerGun.infiniteAmmo;
		playerHealth.immune = perk.immunity || playerHealth.immune;

		if ( perk.gunDrop != null && playerWeapons.weapons.Count < 3 )
		{
			playerWeapons.weapons.Add( perk.gunDrop );
			playerWeapons.NewWeapon();
			playerWeapons.specialGun = playerWeapons.weapons[2].GetComponent<Gun>();
		}
	}

	public void ResetPerk( Perk reset )
	{
		//revert modifiers
		playerSpeed.speedMultiplier -= reset.speedMod;
		playerHealth.maxHealth -= reset.maxHealthMod;
		playerHealth.health -= reset.healthMod;
		//playerDamage.damageMultiplier -= reset.damageMod;
		//playerGun.cooldown -= reset.fireRateMod;
		//playerGun.amountOfMagazines -= reset.magazinesMod;
		//playerGun.reloadSpeed -= reset.reloadMod;
		/*
		if ( reset.infiniteAmmo )
		{
		    playerGun.infiniteAmmo = false;
		}
		*/
		if ( reset.immunity )
		{
			playerHealth.immune = false;
		}
	}
}