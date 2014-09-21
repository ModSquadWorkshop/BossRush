using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour 
{
	public Transform lookTarget;
	public Transform playerModel;

	public float baseSpeed = 200.0f;
	public float speedMultiplier = 1.0f;

	public float lookSpeed = 10.0f;

	private float _speed;

	void Start() 
	{
		_speed = baseSpeed * speedMultiplier;
	}

	void Update() 
	{
		// cardinal movement
		transform.Translate( new Vector3( Input.GetAxis( "Horizontal" ) * Time.deltaTime * _speed * speedMultiplier,
		                                  0.0f,
		                                  Input.GetAxis( "Vertical" )   * Time.deltaTime * _speed * speedMultiplier ) );

		// handle mouse input
		lookTarget.Translate( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) ) * lookSpeed );

		// handle game pad look
		float lookX = Input.GetAxis( "Look Horizontal" );
		float lookY = Input.GetAxis( "Look Vertical" );
		if ( lookX != 0.0f || lookY != 0.0f )
		{
			// hide look target
			//lookTarget.renderer.enabled = false;

			lookTarget.localPosition = new Vector3( lookX, lookTarget.localPosition.y, lookY );
		}

		// don't actually rotate the root Player object,
		// rotate the model.
		playerModel.transform.LookAt( lookTarget );
	}

	public float speed
	{
		get { return _speed * speedMultiplier; }
		set { _speed = value; }
	}
}
