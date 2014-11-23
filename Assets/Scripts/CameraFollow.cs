using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float offset;
	public float followSpeed = 1.0f;

	[Tooltip( "The amount of time the camera will take to pull back after the boss (or player) dies." )]
	public float cameraPullbackTime;
	[Tooltip( "The distance the camera will pull back to view the field after the battle ends." )]
	public float pullbackDistance;

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

	public void PullBackFromTarget( GameObject target )
	{
		iTween.MoveTo( gameObject, target.transform.position - transform.forward * pullbackDistance, cameraPullbackTime );
		enabled = false;
	}

	void TargetDeathCallback( GameObject gameObject )
	{
		Destroy( this );
	}
}
