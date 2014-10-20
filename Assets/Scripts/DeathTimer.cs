using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour 
{
	private Timer _death;
	public float deathTime;
	// Use this for initialization
	void Start () 
	{
		_death = new Timer( deathTime, 1 );
		_death.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_death.Update();
		if ( _death.IsComplete() )
		{
			Destroy( this.gameObject );
		}
	}
}
