using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float speed = 150.0f;            // in world units
	public float maxDistance = 500.0f;      // in world units
	public float maxTime = 8.0f;            // in seconds

	public GameObject shrapnel;
	public bool reflectShrapnel;

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
		// or if it has lived longer than its max life time, it is destroyed
		if ( ( transform.position - _startPoint ).sqrMagnitude > _maxDistanceSquared | _elapsedTime > maxTime )
		{
			GetComponent<DeathSystem>().Kill();
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
			if ( reflectShrapnel )
			{
				shrapnel = Instantiate( shrapnel,
				                        collision.contacts[0].point + collision.contacts[0].normal,
				                        Quaternion.LookRotation( -Vector3.Reflect( -transform.forward, Vector3.up ) ) ) as GameObject;
			}
			else
			{
				shrapnel = Instantiate( shrapnel,
				                        collision.contacts[0].point + collision.contacts[0].normal,
				                        Quaternion.LookRotation( -transform.forward ) ) as GameObject;
			}
		}
	}
}
