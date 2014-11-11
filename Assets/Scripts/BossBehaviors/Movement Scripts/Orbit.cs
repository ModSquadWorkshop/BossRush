using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour, ITargetBasedMovement
{
	//public GameObject targetObj;
	private Transform _target;
	private Vector3 _distance;
	private Vector3 _lookAt;

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

	// Use this for initialization
	void Start () 
	{
		_radius = Random.Range( radiusMin, radiusMax );
		minionGun.GetComponent<Weapon>().cooldown = Random.Range( cooldownMin, cooldownMax );
		_rotationSpeed = rotationSpeeds[Random.Range( 0, rotationSpeeds.Length )];
		_travelSpeed = travelSpeeds[Random.Range( 0, travelSpeeds.Length )];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( _target != null )
		{
			transform.RotateAround( _target.position, Vector3.up, _rotationSpeed * Time.deltaTime );
			_distance = (transform.position - _target.position).normalized * _radius + _target.position;
			transform.position = Vector3.MoveTowards( transform.position, _distance, Time.deltaTime * _travelSpeed );
			minionGun.transform.rotation = Quaternion.LookRotation( _target.position - minionGun.transform.position );
			if ( !minionGun.isOnCooldown )
			{
				//Debug.Log( "FIRE" );
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
