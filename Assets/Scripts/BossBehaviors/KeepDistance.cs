using UnityEngine;
using System.Collections;

public class KeepDistance : MonoBehaviour
{
	public Transform target;
	public float distance;
	public float moveSpeed;

	private float _sqrDistance;
	private Vector3 _movement;

	void Start()
	{
		HealthSystem targetHealth = target.gameObject.GetComponent<HealthSystem>();
		if ( targetHealth != null )
		{
			targetHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( TargetDeath ) );
		}

		_sqrDistance = distance * distance;
	}

	void Update()
	{
		// move towards if too far, move away if too close
		float currentSqrDistance = Vector3.SqrMagnitude( transform.position - target.position );
		_movement = ( currentSqrDistance > _sqrDistance ) ? target.position - transform.position : transform.position - target.position;
		_movement = Vector3.Normalize( _movement );
	}

	void FixedUpdate()
	{

		rigidbody.AddForce( _movement * moveSpeed * Time.fixedDeltaTime * rigidbody.mass * 150.0f );
	}

	public void TargetDeath( HealthSystem targetHealth )
	{
		Destroy( this );
	}
}
