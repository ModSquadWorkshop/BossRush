using UnityEngine;
using System.Collections;

public class MortarAttack : MonoBehaviour 
{
	public GameObject mortar;

	public float speedMin = 20.0f;
	public float speedMax = 40.0f;
	public float minDistance = 0.0f;
	public float maxDistance = 100.0f;
	public float damage = 100.0f;

	public void Launch( int amountMortars )
	{
		for ( int i = 0; i < amountMortars; i++ )
		{
			Mortar m = Instantiate( mortar ) as Mortar;
			m.Init( this.transform.position,
					RandomTargetPosition(), 
					50.0f, 
					1.0f / Random.Range( speedMin, speedMax ),
					damage);
		}
	}

	private Vector3 RandomTargetPosition()
	{
		Vector3 pos = Random.insideUnitCircle * Random.Range( minDistance, maxDistance );
		pos += this.transform.position;
		return pos;
	}
}