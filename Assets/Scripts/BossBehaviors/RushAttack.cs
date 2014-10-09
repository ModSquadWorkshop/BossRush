using UnityEngine;
using System.Collections;

public class RushAttack : PhysicsMovement
{
	public delegate void RushEndCallback();

	public float moveDistance;
	public float attackDamage;

	[HideInInspector] public Transform target;

	private Vector3 _startPos;
	private RushEndCallback _rushEndCallback;

	public void Awake()
	{
		// ensure that _rushEndCallback isn't null
		_rushEndCallback = delegate() { };
	}

	public void OnEnable()
	{
		_movement = Vector3.Normalize( target.position - transform.position );
		_startPos = transform.position;
	}

	public void Update()
	{
		if ( ( _startPos - transform.position ).sqrMagnitude > moveDistance * moveDistance )
		{
			// disable self and notify listeners
			enabled = false;
			_rushEndCallback();
			_rushEndCallback = delegate() { };
		}
	}

	public void RegisterCallback( RushEndCallback callback )
	{
		_rushEndCallback += callback;
	}
}
