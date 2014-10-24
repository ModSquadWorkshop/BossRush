using UnityEngine;
using System.Collections;

public class NavigateTowardsTarget : MonoBehaviour, ITargetBasedMovement
{
	private Transform _target;
	private NavMeshAgent _agent;

	public virtual void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	public void OnEnable()
	{
		_agent.enabled = true;
		Invoke( "ResetTarget", 0.05f );
	}

	public void OnDisable()
	{
		CancelInvoke();
		_agent.enabled = false;
	}

	public void ResetTarget()
	{
		_agent.SetDestination( _target.position );
		Invoke( "ResetTarget", 0.5f );
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
