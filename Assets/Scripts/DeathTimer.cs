using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour
{
	private Timer _death;
	public float deathTime;
	// Use this for initialization
	void Start ()
	{
		if ( deathTime > 0.0f )
		{
			_death = new Timer( deathTime, 1 );
			_death.Start();
		}
		else
		{
			// don't even bother running the timer
			enabled = false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		_death.Update();
		if ( _death.complete )
		{
			GetComponent<DeathSystem>().Kill();
		}
	}
}
