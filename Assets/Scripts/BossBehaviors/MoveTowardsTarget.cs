using UnityEngine;
using System.Collections;

public class MoveTowardsTarget : MonoBehaviour
{
	public Transform target;
	public float speed = 50.0f;

	public virtual void Start()
	{
		HealthSystem targetHealth = target.gameObject.GetComponent<HealthSystem>();
		if ( targetHealth != null )
		{
			targetHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( TargetDeath ) );
		}
	}

	public virtual void Update()
	{
		transform.position = Vector3.MoveTowards( transform.position, target.position, Time.deltaTime * speed );
	}

	public void TargetDeath( HealthSystem targetHealth )
	{
		// destroy follow script without destroying object
		Destroy( this );
	}
}
