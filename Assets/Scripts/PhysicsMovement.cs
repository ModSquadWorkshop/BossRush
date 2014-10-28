using UnityEngine;
using System.Collections;

public abstract class PhysicsMovement : MonoBehaviour
{
	public float maxSpeed;
	public float acceleration;

	/**
	 * \brief Hides the default MonoBehavior.rigidbody property for performance reasons.
	 *
	 * \details The default MonoBehavior.rigidbody property uses GetComponent() in the background,
	 * making it inefficient in cases where the rigidbody needs to be accessed every frame.
	 * This variable hides the original member property with a reference to the rigidbody
	 * that is retrieved once in the Awake() method.
	 */
	protected new Rigidbody rigidbody;
	protected new Transform transform;

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

	public virtual void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		transform = GetComponent<Transform>();
	}

	public void FixedUpdate()
	{
		if ( rigidbody.velocity.sqrMagnitude < maxSpeed * maxSpeed )
		{
			rigidbody.velocity += _movement * acceleration * Time.fixedDeltaTime;
		}
	}
}
