using UnityEngine;
using System.Collections;

public class NavigateTowardsTarget : MonoBehaviour, ITargetBasedMovement
{
	public bool movingTarget;

	private Transform _target;
	private NavMeshAgent _agent;

	public virtual void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	public void OnEnable()
	{
		_agent.enabled = true;
		ResetTarget();
	}

	public void OnDisable()
	{
		CancelInvoke();
		_agent.enabled = false;
	}

	public void ResetTarget()
	{
		if ( _target != null )
		{
			_agent.SetDestination( _target.position );
		}
		if ( movingTarget )
		{
			Invoke( "ResetTarget", 0.2f );
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
			ResetTarget();
		}
	}
}
