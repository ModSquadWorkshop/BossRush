using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float speed = 150.0f;            // in world units
	public float maxDistance = 500.0f;      // in world units
	public float maxTime = 8.0f;            // in seconds

	public GameObject shrapnel;
	public bool reflectShrapnel;

	void OnEnable()
	{
		Invoke( "TimeUp", maxTime );
	}

	void OnDisable()
	{
		CancelInvoke();
	}

	void TimeUp()
	{
		GetComponent<DeathSystem>().Kill();
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
				Vector3 forward = transform.forward;
				forward.y = 0.0f;
				shrapnel = Instantiate( shrapnel,
				                        collision.contacts[0].point + collision.contacts[0].normal,
				                        Quaternion.LookRotation( forward ) ) as GameObject;
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
