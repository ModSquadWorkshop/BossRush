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
	private Vector3 _center;
	public float cameraBack;

	//mouse aim shift variables
	private Vector3 _gamePadLook;
	private Vector3 _mouseLook;
	private Vector3 _dist;
	private Vector3 _mouseDir;

	private bool _mouseMoved;
	private bool _padMoved;

	public float lowerLimit;
	public float upperLimit;

	void Awake()
	{
		transform = GetComponent<Transform>();
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
		_gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ), 0.0f, Input.GetAxis( "Look Vertical" ) );
		_mouseLook = new Vector3 ( Input.mousePosition.x , 0.0f, Input.mousePosition.y );
		_dist = _mouseLook - _center;
		_mouseDir = _dist / _dist.magnitude;

		_mouseMoved = ( Input.mousePosition.x < Screen.width * lowerLimit || Input.mousePosition.x > Screen.width * upperLimit ) ||
		              ( Input.mousePosition.y < Screen.height * lowerLimit || Input.mousePosition.y > Screen.height * upperLimit );

		_padMoved = new Vector3 ( Input.GetAxis( "Look Horizontal" ), 0.0f ,
		                          Input.GetAxis( "Look Vertical" ) ).sqrMagnitude > 0.0f;

		Vector3 lookOffset = Vector3.zero;

		if ( _padMoved )
		{
			lookOffset = _gamePadLook * padOffset;
		}
		else if ( _mouseMoved )
		{
			lookOffset = _mouseDir * mouseOffset;
		}

		transform.position = Vector3.Lerp( transform.position,
		                                   followTarget.position + lookOffset + ( -transform.forward * cameraBack ),
		                                   followSpeed * Time.deltaTime );
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
