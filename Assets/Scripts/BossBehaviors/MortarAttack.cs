using UnityEngine;
using System.Collections;

public class MortarAttack : MonoBehaviour 
{
	public GameObject mortar;
	public GameObject targetMarker;

	public float speedMin = 5.0f;
	public float speedMax = 20.0f;
	public float minDistance = 10.0f;
	public float maxDistance = 15.0f;
	public float damage = 100.0f;

	public void Launch( int amountMortars )
	{
		for ( int i = 0; i < amountMortars; i++ )
		{
			GameObject mortarObject = Instantiate( mortar ) as GameObject;
			Mortar m = mortarObject.GetComponent<Mortar>();
			Vector3 targetPos = RandomTargetPosition();

			m.Init( this.gameObject.transform.position,
					targetPos, 
					100.0f, // arc height
					100.0f / Random.Range( speedMin, speedMax ), // speed/time
					damage,
					targetMarker );
		}
	}

	private Vector3 RandomTargetPosition()
	{
		Vector3 pos = Random.insideUnitCircle * Random.Range( minDistance, maxDistance );
		pos.z = pos.y; // Random.insideUnitCircle returns a 2D vector with (x, y), so we swap y with z for an accurate 3D position
		pos += this.gameObject.transform.position; // offset the psosition to the origin of the mortar launcher
		pos.y = 0.0f;
		return pos;
	}
}