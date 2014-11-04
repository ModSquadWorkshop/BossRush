using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour 
{
	//public GameObject targetObj;
	private Transform _target;
	private Vector3 _distance;
	private Vector3 _lookAt;

	public float radius;
	public float rotationSpeed;
	public float travelSpeed;
	public Gun minionGun;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( _target != null )
		{
			transform.RotateAround( _target.position, Vector3.up, rotationSpeed * Time.deltaTime );
			_distance = (transform.position - _target.position).normalized * radius + _target.position;
			transform.position = Vector3.MoveTowards( transform.position, _distance, Time.deltaTime * travelSpeed );
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
