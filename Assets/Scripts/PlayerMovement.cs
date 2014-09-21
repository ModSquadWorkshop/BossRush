using UnityEngine;
using System.Collections;

sealed public class PlayerMovement : MonoBehaviour 
{
	public float baseSpeed = 200.0f;
	public float speedMultiplier = 1.0f;

	private float _speed;

	void Start() 
	{
		_speed = baseSpeed * speedMultiplier;
	}

	void Update() 
	{
		// cardinal movement

		Vector3 position = this.transform.position;

		position.x = this.transform.position.x + Input.GetAxis( "Horizontal" ) * Time.deltaTime * _speed * speedMultiplier;
		position.z = this.transform.position.z + Input.GetAxis( "Vertical" )   * Time.deltaTime * _speed * speedMultiplier;

		this.transform.position = position;

		// Look at mouse

		Plane plane = new Plane( Vector3.up, this.transform.position );
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		
		float hitDistance = 0.0f;
		
		if ( plane.Raycast( ray, out hitDistance ) ) 
		{
			Vector3 targetPoint = ray.GetPoint( hitDistance );
			Quaternion targetRotation = Quaternion.LookRotation( targetPoint - this.transform.position );
			
			//this.transform.rotation = Quaternion.Slerp( this.transform.rotation, targetRotation, Time.deltaTime * speed );
			this.transform.rotation = targetRotation;
		}
	}

	public float speed 
	{
		get { return _speed * speedMultiplier; }
		set { _speed = value; }
	}
}