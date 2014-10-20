using UnityEngine;
using System.Collections;

public class MoveTowardsTarget : PhysicsMovement
{
	private Transform _target;

	public virtual void Update()
	{
		_movement = Vector3.Normalize( _target.position - transform.position );
	}

	public void TargetDeathCallback( GameObject gameObject )
	{
		// There are cases where the death callback
		// might be called after the object has been destroyed.
		// In theory we could just de-register when we die,
		// but that has its own issues. So instead we have
		// to check if this object still exists.
		if ( this != null )
		{
			// destroy follow script without destroying object
			CancelInvoke();
			Destroy( this );
		}
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

			// register for death callback
			DeathSystem targetDeath = _target.GetComponent<DeathSystem>();
			if ( targetDeath != null )
			{
				targetDeath.RegisterDeathCallback( TargetDeathCallback );
			}
		}
	}
}
