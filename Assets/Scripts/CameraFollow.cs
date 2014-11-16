using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float offset;
	public float followSpeed = 1.0f;

	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
	}

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
		transform.position = Vector3.Lerp( transform.position, followTarget.position + ( -transform.forward * offset ), followSpeed * Time.deltaTime );
	}

	void TargetDeathCallback( GameObject gameObject )
	{
		Destroy( this );
	}
}
