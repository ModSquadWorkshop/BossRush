using UnityEngine;
using System.Collections;

public class PerkTime : MonoBehaviour
{
    //time a perk lasts (if using a timer);
    public float perkLength;
    public Timer perkTimer;
    //index of perk within the system
    public int perkNumber;

    public bool started = false;

    public void BeginPerk()
    {
        perkTimer = new Timer( perkLength, 1 );
        perkTimer.Start();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ( started )
        {
            perkTimer.Update();

            if ( perkTimer.IsComplete() )
            {
                started = false;
                EndPerk();
            }
        }
    }

    public void EndPerk()
    {
        this.gameObject.GetComponent<PerkSystem>().RemovePerk( perkNumber );
        Destroy( this );
    }
}