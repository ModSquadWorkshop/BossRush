using UnityEngine;
using System.Collections;

public class KeepDistance : PhysicsMovement
{
	public Transform target;
	public float distance;

	void Start()
	{
		HealthSystem targetHealth = target.gameObject.GetComponent<HealthSystem>();
		if ( targetHealth != null )
		{
			targetHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( TargetDeath ) );
		}
	}

	void Update()
	{
		// move towards if too far, move away if too close
		float currentSqrDistance = Vector3.SqrMagnitude( transform.position - target.position );
		_movement = ( currentSqrDistance > distance * distance ) ? target.position - transform.position : transform.position - target.position;
		_movement = Vector3.Normalize( _movement );
	}

	public void TargetDeath( HealthSystem targetHealth )
	{
		Destroy( this );
	}
}
