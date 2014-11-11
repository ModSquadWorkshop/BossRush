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
				//currentPerk.Refresh();
				RefreshPerk( currentPerk );
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
			if ( perk.gunDrop == null )
			{
				perk.Begin();
			}
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
		if ( perk.gunDrop != null && perk.duration <= 0 )
		{
			//Debug.Log( "ADD AMMO" );
			if ( playerWeapons.DetermineType() )
			{
				Gun special = playerWeapons.weapons[2].GetComponent<Gun>();
				special.RefreshAmmo();
			}
			else if ( !playerWeapons.DetermineType() )
			{
				//Debug.Log( "ADD TIME" );
				BeamWeapon special = playerWeapons.weapons[2].GetComponent<BeamWeapon>();
				special.ResetTimer();
			}
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
		playerWeapons.SetBuffs( perk.fireRateMod, perk.damageMod, perk.reloadMod, perk.infiniteAmmo );
		playerHealth.immune = perk.immunity || playerHealth.immune; //create shield or change healthbar if true
		
		if ( perk.gunDrop != null && playerWeapons.weapons.Count <= 3 )
		{
			if ( playerWeapons.weapons.Count == 3 )
			{
				playerWeapons.RemoveSpecial();
			}
			playerWeapons.weapons.Add( perk.gunDrop );
			playerWeapons.perk = perk;
			playerWeapons.NewWeapon();
		}
	}

	public void ResetPerk( Perk reset )
	{
		//revert modifiers
		playerSpeed.speedMultiplier -= reset.speedMod;
		playerHealth.maxHealth -= reset.maxHealthMod;
		playerHealth.health -= reset.healthMod;
		playerWeapons.RevertBuffs( reset.fireRateMod, reset.damageMod, reset.reloadMod, reset.infiniteAmmo );
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