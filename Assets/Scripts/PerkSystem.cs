using UnityEngine;
using System.Collections;

public class PerkSystem : MonoBehaviour
{
    public Perk[] startingPerks;
    private Hashtable _perks;

    void Start()
    {
        _perks = new Hashtable();

        for ( int i = 0; i < startingPerks.Length; i++ )
        {
            AddPerk( startingPerks[i] );
        }
    }

    public void AddPerk(Perk perk)
    {
        _perks[perk] = true;
        SetPerk( perk );
        CreateTimer( perk );
    }

    public void SetPerk( Perk perk )
    {
        //check type of perk and apply modifier
        if ( perk.speedMod != 0f )
        {
            this.gameObject.GetComponent<PlayerMovement>().speedMultiplier += perk.speedMod;
        }
        else if ( perk.maxHealthMod != 0f )
        {
            this.gameObject.GetComponent<HealthSystem>().maxHealth += perk.maxHealthMod;
        }
        else if ( perk.damageMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>().damageMultiplier += perk.damageMod;
        }
        else if ( perk.fireRateMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().cooldown += perk.fireRateMod;
        }
        else if ( perk.immunity )
        {
            this.gameObject.GetComponent<HealthSystem>().immune = true;
        }
        else if ( perk.healthMod != 0f )
        {
            this.gameObject.GetComponent<HealthSystem>().health += perk.healthMod;
        }
        else if ( perk.magazinesMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().amountOfMagazines += perk.magazinesMod;
        }
        else if ( perk.infiniteAmmo )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().infiniteAmmo = true;
        }
        else if ( perk.reloadMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().reloadSpeed += perk.reloadMod;
        }
    }

    public void CreateTimer( Perk perk )
    {
        if ( perk.length > 0f )
        {
            PerkTime timer = this.gameObject.AddComponent<PerkTime>();
            timer.perkLength = perk.length;
            timer.perk = perk;
            timer.Begin();
        }
    }

    public void ResetPerk( Perk reset )
    {
        //finds the perk in actives and removes the modifier based on type
        if ( reset.speedMod != 0f )
        {
            this.gameObject.GetComponent<PlayerMovement>().speedMultiplier -= reset.speedMod;
        }
        else if ( reset.maxHealthMod != 0f )
        {
            this.gameObject.GetComponent<HealthSystem>().maxHealth -= reset.maxHealthMod;
        }
        else if ( reset.damageMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>().damageMultiplier -= reset.damageMod;
        }
        else if ( reset.fireRateMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().cooldown -= reset.fireRateMod;
        }
        else if ( reset.immunity )
        {
            this.gameObject.GetComponent<HealthSystem>().immune = false;
        }
        else if ( reset.healthMod != 0f )
        {
            this.gameObject.GetComponent<HealthSystem>().health -= reset.healthMod;
        }
        else if ( reset.magazinesMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().amountOfMagazines -= reset.magazinesMod;
        }
        else if ( reset.infiniteAmmo )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().infiniteAmmo = false;
        }
        else if ( reset.reloadMod != 0f )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().reloadSpeed -= reset.reloadMod;
        }
    }

    public bool IsActive( Perk perk )
    {
        return (bool)_perks[perk];
    }

    public void Clear()
    {
        PerkTime[] timers = gameObject.GetComponents<PerkTime>();
        foreach(PerkTime time in timers)
        {
            time.End();
        }
        _perks.Clear();
    }

    public void RemovePerk( Perk perk )
    {
        ResetPerk( perk );
        _perks[perk] = false;
    }

}