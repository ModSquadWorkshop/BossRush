using UnityEngine;
using System.Collections;

public class MoveTowardsTargetFlanking : MoveTowardsTarget
{
	public Vector3 controlPoint;

	[Range( 0.0f, 1.0f )]
	public float weight;

	private float _weight = 1.0f;

	public override void Update()
	{
		// calcluate movement vector
		Vector3 dirToTarget = Vector3.Normalize( target.position - transform.position );
		Vector3 dirToControlPoint = Vector3.Normalize( controlPoint - transform.position );
		_movement = _weight * dirToControlPoint + ( 1.0f - _weight ) * dirToTarget;

		// update weight
		_weight = Mathf.Clamp( _weight - ( 1.0f - weight ) * Time.deltaTime, 0.0f, 1.0f );
	}
}
