using UnityEngine;
using System.Collections;

public class PerkTime : MonoBehaviour
{
    //time a perk lasts (if using a timer);
    public float perkLength;
    private Timer perkTimer;
    //index of perk within the system
    public Perk perk;

    // Update is called once per frame
    void Update()
    {
        perkTimer.Update();
        if (perkTimer.IsComplete())
        {
            End();
        }
    }

    public void Begin()
    {
        perkTimer = new Timer( perkLength, 1 );
        perkTimer.Start();
    }

    public void End()
    {
        this.gameObject.GetComponent<PerkSystem>().RemovePerk( perk );
        Destroy( this );
    }
}