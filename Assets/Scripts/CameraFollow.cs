using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float padOffset;
	public float mouseOffset;
	public float followSpeed = 1.0f;

	[Tooltip( "The amount of time the camera will take to pull back after the boss (or player) dies." )]
	public float cameraPullbackTime;
	[Tooltip( "The distance the camera will pull back to view the field after the battle ends." )]
	public float pullbackDistance;

	private new Transform transform;
	private Vector3 _heightMod;
	private Vector3 _center;
	private float height;
	public float cameraBack;

	void Awake()
	{
		transform = GetComponent<Transform>();
		height = 80f / padOffset;
		_heightMod = new Vector3( 0f, 80f, 0f );
		_center = new Vector3( Screen.width / 2, 0.0f, Screen.height / 2 );
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
		
		Vector3 mouseLook = new Vector3 ( Input.mousePosition.x , 0.0f, Input.mousePosition.y );

		Vector3 dist = mouseLook - _center;

		Vector3 mouseDir = dist / dist.magnitude;

		bool mouseMoved = Input.mousePosition.x != Screen.width/2 || Input.mousePosition.y != Screen.height / 2;

		bool padMoved = new Vector3 ( Input.GetAxis( "Look Horizontal" ), 0.0f , 
									  Input.GetAxis( "Look Vertical" ) ).sqrMagnitude > 0.0f;
		//Debug.Log( gamePadLook );
		//Debug.Log( "OTHER" );
		//Debug.Log( -transform.forward * offset );
		
		if ( padMoved )
		{
			transform.position = Vector3.Lerp( transform.position, followTarget.position + (gamePadLook * padOffset) + ( Vector3.back * cameraBack ), followSpeed * Time.deltaTime );
		}
		else if ( mouseMoved )
		{
			transform.position = Vector3.Lerp( transform.position, followTarget.position + (mouseDir * mouseOffset) + _heightMod + (Vector3.back * cameraBack), followSpeed * Time.deltaTime );
		}
		else
		{
			transform.position = Vector3.Lerp( transform.position, followTarget.position + _heightMod + ( Vector3.back * cameraBack ) , followSpeed * Time.deltaTime );
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
