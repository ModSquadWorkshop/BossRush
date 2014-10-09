using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float offset;
	public float followSpeed = 1.0f;
	public float shakeForce;
	public float shakePerDamage;

	void Start()
	{
		HealthSystem targetHealth = followTarget.gameObject.GetComponent<HealthSystem>();
		if ( targetHealth != null )
		{
			targetHealth.RegisterDeathCallback( TargetDeathCallback );
		}
	}

	void Update()
	{
		transform.position = Vector3.Lerp( transform.position, followTarget.position + new Vector3( 0.0f, offset, 0.0f ), followSpeed * Time.deltaTime );
	}

	void TargetDeathCallback( HealthSystem targetHealth )
	{
		Destroy( this );
	}

	public void Shake( float ammount )
	{
		iTween.ShakePosition( Camera.main.gameObject, Vector3.left * ( shakeForce + ( ammount * shakePerDamage ) ), 0f );
	}
}
