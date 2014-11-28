using UnityEngine;
using System.Collections;

public class KeepDistance : PhysicsMovement
{
	public Transform target;
	public float distance;

	void Start()
	{
		DeathSystem targetDeath = target.gameObject.GetComponent<DeathSystem>();
		if ( targetDeath != null )
		{
			targetDeath.RegisterDeathCallback( TargetDeath );
		}
	}

	void Update()
	{
		// move towards if too far, move away if too close
		float currentSqrDistance = Vector3.SqrMagnitude( transform.position - target.position );
		_movement = ( currentSqrDistance > distance * distance ) ? target.position - transform.position : transform.position - target.position;
		_movement = Vector3.Normalize( _movement );
	}

	public void TargetDeath( GameObject gameObject )
	{
		Destroy( this );
	}
}
