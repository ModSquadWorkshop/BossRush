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
	private Vector3 heightMod;
	private float height;
	public float cameraBack;

	void Awake()
	{
		transform = GetComponent<Transform>();
		height = 80f / offset;
		heightMod = new Vector3( 0f, 80f, 0f );
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
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), height , Input.GetAxis( "Look Vertical" ) );
		//Debug.Log( gamePadLook );
		//Debug.Log( "OTHER" );
		//Debug.Log( -transform.forward * offset );

		if ( Input.GetAxis( "Look Horizontal" ) != 0f || Input.GetAxis( "Look Vertical" ) != 0f )
		{
			transform.position = Vector3.Lerp( transform.position, followTarget.position + (gamePadLook * offset) + ( Vector3.back * cameraBack ), followSpeed * Time.deltaTime );
		}
		else
		{
			transform.position = Vector3.Lerp( transform.position, followTarget.position + heightMod + ( Vector3.back * cameraBack ) , followSpeed * Time.deltaTime );
		}
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
