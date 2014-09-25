using UnityEngine;
using System.Collections;

sealed public class MoveTowardsTarget : MonoBehaviour
{
	public GameObject target;
	public float speed = 50.0f;

	void Start()
	{
		HealthSystem targetHealth = target.GetComponent<HealthSystem>();
		if ( targetHealth != null )
		{
			targetHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( TargetDeath ) );
		}
	}

	void Update()
	{
		this.transform.position = Vector3.MoveTowards( this.transform.position, target.transform.position, Time.deltaTime * speed );
	}

	public void TargetDeath( HealthSystem targetHealth )
	{
		// destroy follow script without destroying object
		Destroy( this );
	}
}
