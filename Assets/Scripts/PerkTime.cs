using UnityEngine;
using System.Collections;

public class PerkTime : MonoBehaviour 
{
	//time a perk lasts (if using a timer);
	public float perkLength;
	
	public Timer perkTimer;

	public bool started = false;

	public void BeginPerk ()
	{
		perkTimer = new Timer( perkLength );
		perkTimer.Start();
		started = true;
	}
		
	// Update is called once per frame
	void Update () 
	{
		if(started)
		{
			perkTimer.Update();

			if( perkTimer.IsTicked() )
			{
				this.GetComponentInParent<HealthSystem>().immune = false;
				perkTimer.Stop();
				started = false;
			}
		}
	}
}