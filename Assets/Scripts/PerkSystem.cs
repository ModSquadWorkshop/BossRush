using UnityEngine;
using System.Collections;

public class PerkSystem : MonoBehaviour
{
    public static int perkLimit = 20;

    PerkTime curr;
    Perk[] actives = new Perk[perkLimit];

    public void AddPerk( Perk a )
    {
        for ( int i = 0; i < perkLimit; i++ )
        {
            if ( actives[i] == null || !actives[i].isActive )
            {
                actives[i] = a;
                a.isActive = true;
                a.perkInd = i;
                Debug.Log(a.perkInd);
                break;
            }
        }
        SetPerk( a );
        CreateTimer( a );
    }

    public void SetPerk( Perk s )
    {
        //check type of perk and apply modifier
        if ( s.speed )
        {
            this.gameObject.GetComponent<PlayerMovement>().speedMultiplier += s.mod;
        }
        else if ( s.maxHealth )
        {
            this.gameObject.GetComponent<HealthSystem>().maxHealth += s.mod;
        }
        else if ( s.damage )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>().damageMultiplier += s.mod;
        }
        else if ( s.fireRate )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().cooldown += s.mod;
        }
        else if ( s.immunity )
        {
            this.gameObject.GetComponent<HealthSystem>().immune = true;
        }
        else if ( s.health )
        {
            this.gameObject.GetComponent<HealthSystem>().health += s.mod;
        }
        else if ( s.ammo )
        {
            //this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().(ammovar) += s.mod;
        }
    }

    public void CreateTimer( Perk c )
    {
        if ( c.length > 0 )
        {
            curr = this.gameObject.AddComponent<PerkTime>();
            curr.perkNumber = c.perkIndex;
            curr.perkLength = c.length;
            curr.BeginPerk();
        }
    }

    public void ResetPerk( Perk reset )
    {
        //finds the perk in actives and removes the modifier based on type
        if ( reset.speed )
        {
            this.gameObject.GetComponent<PlayerMovement>().speedMultiplier -= reset.mod;
        }
        else if ( reset.maxHealth )
        {
            this.gameObject.GetComponent<HealthSystem>().maxHealth -= reset.mod;
        }
        else if ( reset.damage )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<DamageSystem>().damageMultiplier -= reset.mod;
        }
        else if ( reset.fireRate )
        {
            this.gameObject.GetComponent<WeaponSystem>().currentWeapon.GetComponent<Gun>().cooldown -= reset.mod;
        }
        else if ( reset.immunity )
        {
            this.gameObject.GetComponent<HealthSystem>().immune = false;
        }

    }

    public void RemovePerk( int n )
    {
        Perk r = actives[n];
        ResetPerk( r );
        actives[n].isActive = false;
        actives[n] = null;
    }

}