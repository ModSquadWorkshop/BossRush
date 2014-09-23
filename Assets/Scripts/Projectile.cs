using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float speed = 150.0f;            // in world units
	public float maxDistance = 500.0f;      // in world units
	public float maxTime = 8.0f;            // in seconds

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
		if ( ( transform.position - _startPoint ).sqrMagnitude > _maxDistanceSquared )
		{
			Destroy( this.gameObject );
		}

		// if the projectile has lasted longer the its max life time, it is destroyed
		if ( _elapsedTime > maxTime )
		{
			Destroy( this.gameObject );
		}
	}
}