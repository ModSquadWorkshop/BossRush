using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour
{
    //bools to determine what is being modified by a perk object
    public bool immunity;
    public bool speed;
    public bool fireRate;
    public bool maxHealth;
    public bool health;
    public bool damage;
    public bool ammo;

    public bool isActive;

    //ammount to change various stats by
    public float mod;

    //time a perk lasts (if using a timer);
    public float length;

    public int perkInd;
}