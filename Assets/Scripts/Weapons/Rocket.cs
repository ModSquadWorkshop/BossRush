using UnityEngine;
using System.Collections;

public class Rocket : Projectile
{
	public float velocityIncrease = 1.1f;

	void Update()
	{
		rigidbody.velocity *= velocityIncrease;
	}
}
