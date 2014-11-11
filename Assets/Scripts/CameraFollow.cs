using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float offset;
	public float followSpeed = 1.0f;

	void Start()
	{
		DeathSystem targetDeath = followTarget.gameObject.GetComponent<DeathSystem>();
		if ( targetDeath != null )
		{
			targetDeath.RegisterDeathCallback( TargetDeathCallback );
		}
	}

	void Update()
	{
		transform.position = Vector3.Lerp( transform.position, followTarget.position + new Vector3( 0.0f, offset, 0.0f ), followSpeed * Time.deltaTime );
	}

	void TargetDeathCallback( GameObject gameObject )
	{
		Destroy( this );
	}
}
