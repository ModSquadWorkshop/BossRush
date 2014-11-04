using UnityEngine;
using System.Collections;

public class MortarAttack : MonoBehaviour
{
	public GameObject mortar;
	public GameObject target;
	public GameObject targetMarker;

	public float mortarSpeedMin = 20.0f;
	public float mortarSpeedMax = 40.0f;
	public float mortarMinDistance = 0.0f;
	public float mortarMaxDistance = 20.0f;
	public float delayBetweenMortars = 0.15f;

	public bool allowMultipleLaunches;

	public bool usePredefinedTargetPos;
	public Transform[] predefinedTargetPos;

	private bool _firing = false;

	public void Launch( int numMortars )
	{
		if ( !( !allowMultipleLaunches && _firing ) )
		{
			StartCoroutine( LaunchMortar( numMortars ) );
			_firing = true;
		}
	}

	private IEnumerator LaunchMortar( int numMortars )
	{
		while ( numMortars > 0 )
		{
			Mortar mortarObject = ( Instantiate( mortar ) as GameObject ).GetComponent<Mortar>();
			Vector3 targetPos = GetTargetPosition();

			mortarObject.Init( Random.Range( mortarSpeedMin, mortarSpeedMax ),
			                   transform.position,
			                   target,
			                   targetPos,
			                   targetMarker );

			numMortars--;

			yield return new WaitForSeconds( delayBetweenMortars );
		}
		_firing = false;
	}

	private Vector3 GetTargetPosition()
	{
		Vector3 pos;

		if ( !usePredefinedTargetPos )
		{
			pos = Random.insideUnitCircle * Random.Range( mortarMinDistance, mortarMaxDistance );
			pos.z = pos.y; // Random.insideUnitCircle returns a 2D vector with (x, y), so we swap y with z for an accurate 3D position
			pos += target.transform.position; // offset the psosition to the origin of the targeted object
			pos.y = 0.0f;
		}
		else
		{
			pos = predefinedTargetPos[ Random.Range( 0, predefinedTargetPos.Length ) ].position;
		}

		return pos;
	}
}
