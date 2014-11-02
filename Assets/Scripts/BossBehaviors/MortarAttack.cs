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

	public bool usePredefinedTargetPos;
	public Transform[] predefinedTargetPos;

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
		Vector3 targetPos = GetTargetPosition();

		m.Init( Random.Range( mortarSpeedMin, mortarSpeedMax ),
				this.gameObject.transform.position,
				target,
				targetPos,
				targetMarker );

		_amountMortars--;
		_delayTimer.Reset( true );
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