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

	void Start () 
	{
		_speed = baseSpeed * speedMultiplier;
	}

	void Update () 
	{
		Vector3 position = this.transform.position;

		position.x = transform.position.x + Input.GetAxis( "Horizontal" ) * Time.deltaTime * _speed * speedMultiplier;
		position.z = transform.position.z + Input.GetAxis( "Vertical" )   * Time.deltaTime * _speed * speedMultiplier;

		// handle mouse input
		lookTarget.Translate( new Vector3( Input.GetAxis( "Mouse X" ), 0.0f, Input.GetAxis( "Mouse Y" ) ) * lookSpeed );

		// handle joystick input
		// TODO

		transform.position = position;

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
