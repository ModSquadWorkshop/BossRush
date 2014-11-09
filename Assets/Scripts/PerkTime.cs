using UnityEngine;
using System.Collections;

public class PerkTime : MonoBehaviour
{
	//time a perk lasts (if using a timer);
	public float perkLength;
	private Timer _perkTimer;

	public Perk perk;

	// Update is called once per frame
	void Update()
	{
		_perkTimer.Update();
		if ( _perkTimer.complete )
		{
			End();
		}
	}

	public void Begin()
	{
		_perkTimer = new Timer( perkLength, 1 );
		_perkTimer.Start();
	}

	public void RefreshTimer()
	{
		_perkTimer.Reset();
	}

	public void End()
	{
		this.gameObject.GetComponent<PerkSystem>().RemovePerk( perk );
		this.gameObject.GetComponent<PerkSystem>().timers.Remove( this );
		Destroy( this );
	}
}