using UnityEngine;
using System.Collections;

public abstract class PhysicsMovement : MonoBehaviour
{
	public float maxSpeed;
	public float acceleration;

	/**
	 * \brief The movement vector for the PhysicsMovement object.
	 *
	 * \details Set the value of this variable in Update() to control
	 * the direction of the PhysicsMovement.
	 *
	 * \note This should always be normalized after it is set. If you don't want
	 * to be moving, set it to Vector3.zero.
	 */
	protected Vector3 _movement;

	public void FixedUpdate()
	{
		if ( rigidbody.velocity.sqrMagnitude < maxSpeed * maxSpeed )
		{
			rigidbody.AddForce( _movement * rigidbody.mass * acceleration );
		}
	}
}
