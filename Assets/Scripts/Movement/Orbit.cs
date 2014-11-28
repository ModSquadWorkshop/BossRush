using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour, ITargetBasedMovement
{
	//public GameObject targetObj;
	private Transform _target;
	private Vector3 _distance;

	//movement options
	public float[] rotationSpeeds;
	public float[] travelSpeeds;
	public float radiusMin;
	public float radiusMax;
	public float cooldownMin;
	public float cooldownMax;

	private float _radius;
	private float _rotationSpeed; //negative switches direction
	private float _travelSpeed; //negative spirals towards target

	public Gun minionGun;

	void Start () 
	{
		_radius = Random.Range( radiusMin, radiusMax );
		minionGun.GetComponent<Gun>().cooldown = Random.Range( cooldownMin, cooldownMax );
		_rotationSpeed = rotationSpeeds[Random.Range( 0, rotationSpeeds.Length )];
		_travelSpeed = travelSpeeds[Random.Range( 0, travelSpeeds.Length )];
	}

	void Update() 
	{
		if ( _target != null )
		{
			transform.RotateAround( _target.position, Vector3.up, _rotationSpeed * Time.deltaTime );
			_distance = (transform.position - _target.position).normalized * _radius + _target.position;
			transform.position = Vector3.MoveTowards( transform.position, _distance, Time.deltaTime * _travelSpeed );
			minionGun.transform.rotation = Quaternion.LookRotation( _target.position - minionGun.transform.position );
			if ( !minionGun.isOnCooldown )
			{
				minionGun.PerformPrimaryAttack();
			}
		}
	}

	public Transform target
	{
		get
		{
			return _target;
		}

		set
		{
			_target = value;
		}
	}
}
