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

	private int _amountMortars;
	private Timer _delayTimer;

	void Start()
	{
		_delayTimer = new Timer( delayBetweenMortars, 1 );
	}

	void Update()
	{
		if ( _amountMortars > 0 )
		{
			if ( _delayTimer.running )
			{
				_delayTimer.Update();

				if ( _delayTimer.complete )
				{
					LaunchMortar();
				}
			}
		}
		else
		{
			_delayTimer.Stop();
		}
	}

	public void Launch( int amountMortars )
	{
		_amountMortars = amountMortars;
		_delayTimer.Reset( true );
	}

	private void LaunchMortar()
	{
		GameObject mortarObject = Instantiate( mortar ) as GameObject;
		Mortar m = mortarObject.GetComponent<Mortar>();
		Vector3 targetPos = RandomTargetPosition();

		m.Init( 100.0f / Random.Range( mortarSpeedMin, mortarSpeedMax ), // speed/time/duration
				this.gameObject.transform.position,
				target,
				targetPos,
				targetMarker );

		_amountMortars--;
		_delayTimer.Reset( true );
	}

	private Vector3 RandomTargetPosition()
	{
		Vector3 pos = Random.insideUnitCircle * Random.Range( mortarMinDistance, mortarMaxDistance );
		pos.z = pos.y; // Random.insideUnitCircle returns a 2D vector with (x, y), so we swap y with z for an accurate 3D position
		pos += target.transform.position; // offset the psosition to the origin of the targeted object
		pos.y = 0.0f;
		return pos;
	}
}