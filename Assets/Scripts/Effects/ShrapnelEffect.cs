using UnityEngine;
using System.Collections;

public class ShrapnelEffect : MonoBehaviour
{
	public float force;
	public float radius;

	void Start ()
	{
		// apply an explosion force to each of the object's children
		foreach ( Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>() )
		{
			rigidbody.AddExplosionForce( force, transform.position, radius );
		}
	}
}