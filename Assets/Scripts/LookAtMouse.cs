using UnityEngine;
using System.Collections;

sealed public class LookAtMouse : MonoBehaviour 
{
	public float speed = 5.0f;
	
	void Update () 
	{
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
}
