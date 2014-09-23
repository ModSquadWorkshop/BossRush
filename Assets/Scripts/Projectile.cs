using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float speed = 150.0f;            // in world units
	public float maxDistance = 500.0f;      // in world units
	public float maxTime = 8.0f;            // in seconds

	public GameObject shrapnel;

	private Vector3 _startPoint;
	private float _maxDistanceSquared;
	private float _elapsedTime;

	void Start()
	{
		rigidbody.velocity = transform.forward * speed;
		_startPoint = transform.position;
		_maxDistanceSquared = maxDistance * maxDistance;
		_elapsedTime = 0.0f;
	}

	void Update()
	{
		_elapsedTime += Time.deltaTime;

		// if the projectile has traveled farther than its max distance, it is destroyed
		if ( ( transform.position - _startPoint ).sqrMagnitude > _maxDistanceSquared | _elapsedTime > maxTime )
		{
			Destroy( this.gameObject );
		}
	}

	/**
	 * \brief Called by the DamageSystem when it destroys the particle.
	 */
	public void Explode( Collision collision )
	{
		// create shrapnel
		if ( shrapnel != null )
		{
			shrapnel = Instantiate( shrapnel,
			                        collision.contacts[0].point,
			                        Quaternion.FromToRotation( Vector3.up, collision.contacts[0].normal ) ) as GameObject;
		}
	}
}
