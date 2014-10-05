using UnityEngine;
using System.Collections;

public class MoveTowardsTarget : PhysicsMovement
{
	private Transform _target;

	public virtual void Update()
	{
		_movement = Vector3.Normalize( _target.position - transform.position );
	}

	public void TargetDeath( HealthSystem targetHealth )
	{
		// destroy follow script without destroying object
		Destroy( this );
	}

	public Transform target
	{
		get
		{
			return _target;
		}

		set
		{
			_target = value;

			// re-register for death callback
			HealthSystem targetHealth = _target.gameObject.GetComponent<HealthSystem>();
			if ( targetHealth != null )
			{
				targetHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( TargetDeath ) );
			}
		}
	}
}
