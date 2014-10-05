using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour
{
	public Transform lookTarget;
	public Transform playerModel;

	public float baseSpeed;
	public float speedMultiplier;

	public float lookSpeed;

	void Update()
	{
		// cardinal movement
		Vector3 movement = new Vector3( Input.GetAxis( "Horizontal" ),
		                                0.0f,
		                                Input.GetAxis( "Vertical" ) );
		rigidbody.velocity = movement * baseSpeed * speedMultiplier;

		// handle mouse input
		lookTarget.Translate( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) ) * lookSpeed );

		// handle game pad look
		Vector3 gamePadLook = new Vector3( Input.GetAxis( "Look Horizontal" ),
		                                   0.0f,
		                                   Input.GetAxis( "Look Vertical" ) );
		if ( gamePadLook.sqrMagnitude > 0.0f )
		{
			// hide look target
			//lookTarget.renderer.enabled = false;

			lookTarget.localPosition = gamePadLook;
		}

		// don't actually rotate the root Player object,
		// rotate the model.
		playerModel.transform.LookAt( lookTarget );
	}

	public float speed
	{
		get
		{
			return baseSpeed * speedMultiplier;
		}
	}
}
