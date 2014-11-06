using UnityEngine;
using System.Collections;

public class RushAttack : PhysicsMovement
{
	[HideInInspector] public Transform target;

	public RushAttackSettings settings;

	public delegate void RushEndCallback();
	private RushEndCallback _rushEndCallback;

	public override void Awake()
	{
		base.Awake();

		// ensure that _rushEndCallback isn't null
		_rushEndCallback = delegate() { };
	}

	public void OnEnable()
	{
		_movement = Vector3.Normalize( target.position - transform.position );
		Invoke( "EndRush", settings.rushTime );
	}

	public void EndRush()
	{
		// disable self and notify listeners
		enabled = false;
		_rushEndCallback();
		_rushEndCallback = delegate() { };
	}

	public void RegisterCallback( RushEndCallback callback )
	{
		_rushEndCallback += callback;
	}

	void OnDisable()
	{
		CancelInvoke();
	}
}


[System.Serializable]
public class RushAttackSettings
{
	public float rushTime;
	public float attackDamage;
}