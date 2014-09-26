using UnityEngine;
using System.Collections;

public class MoveTowardsTargetFlanking : MoveTowardsTarget
{
	public Vector3 controlPoint;

	[Range( 0.0f, 1.0f )]
	public float weight;

	public float moveSpeed;

	private float _weightDecay;
	private float _weight;

	public override void Start()
	{
		base.Start();

		_weightDecay = 1.0f - weight;
		_weight = 1.0f;
	}

	public override void Update()
	{
		// calcluate movement vector
		Vector3 dirToTarget = Vector3.Normalize( target.position - transform.position );
		Vector3 dirToControlPoint = Vector3.Normalize( controlPoint - transform.position );
		Vector3 moveDir = _weight * dirToControlPoint + ( 1.0f - _weight ) * dirToTarget;
		rigidbody.AddForce( Vector3.Normalize( moveDir ) * moveSpeed * Time.deltaTime * 750.0f );

		_weight = Mathf.Clamp( _weight - _weightDecay * Time.deltaTime, 0.0f, 1.0f );
	}
}
