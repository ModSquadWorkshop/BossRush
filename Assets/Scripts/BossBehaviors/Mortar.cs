using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour
{
	private float       _speed;
	private Vector3     _targetPos;
	private GameObject  _marker; // the actual instantiated target marker
	private Vector3     _velocity;

	private MortarSettings _settings;

	public void Init( MortarSettings settings, Vector3 startPos )
	{
		_settings = settings;
		transform.position = startPos;

		_settings.target.GetComponent<DeathSystem>().RegisterDeathCallback( TargetDeath );

		// configure all the things
		_speed = Random.Range( _settings.minSpeed, _settings.maxSpeed );
		_targetPos = new Vector3( startPos.x, _settings.arcHeight, startPos.z );
		_speed = Random.Range( _settings.minSpeed, _settings.maxSpeed );
		_velocity = Vector3.Normalize( _targetPos - transform.position ) * _speed;

		// calculate time to apex
		float timeToApex = Vector3.Distance( transform.position, _targetPos ) / _speed;
		Invoke( "OnMidComplete", timeToApex );
	}

	void Update()
	{
		this.gameObject.transform.Translate( _velocity * Time.deltaTime, Space.World );
	}

	void OnMidComplete()
	{
		// calculate the things
		_speed *= _settings.speedIncrease;
		transform.rotation = Quaternion.AngleAxis( Mathf.Atan2( _velocity.y, _velocity.x ) * Mathf.Rad2Deg, Vector3.forward * 90.0f );
		_targetPos = GetTargetPosition();
		_marker = Instantiate( _settings.targetMarker, _targetPos, Quaternion.identity ) as GameObject;
		_velocity = Vector3.Normalize( _targetPos - transform.position ) * _speed;

		// calculate time to target
		float timeToTarget = Vector3.Distance( transform.position, _targetPos ) / _speed;
		Invoke( "OnComplete", timeToTarget );
	}

	void OnComplete()
	{
		_settings.target.GetComponent<DeathSystem>().DeregisterDeathCallback( TargetDeath );
		GetComponent<DeathSystem>().Kill();
		Destroy( _marker );
	}

	private Vector3 GetTargetPosition()
	{
		Vector3 offset = Random.insideUnitCircle * Random.Range( _settings.minTargetOffset, _settings.maxTargetOffset );
		offset.z = offset.y; // Random.insideUnitCircle returns a 2D vector with (x, y), so we swap y with z for an accurate 3D position
		offset.y = 0.0f;

		if ( !_settings.usePredefinedTargetPos )
		{
			// offset the position to the origin of the targeted object
			Vector3 targetDirection = Vector3.Normalize( _settings.target.rigidbody.velocity );
			return offset + _settings.target.transform.position +  targetDirection * _settings.targetLead;
		}
		else
		{
			return offset + _settings.predefinedTargetPos[Random.Range( 0, _settings.predefinedTargetPos.Length )].position;
		}
	}

	void TargetDeath( GameObject obj )
	{
		// don't go through the DeathSystem,
		// we're doing cleanup, not normal gameplay
		Destroy( gameObject );
		if ( _marker != null )
		{
			Destroy( _marker );
		}
		CancelInvoke();
	}
}

[System.Serializable]
public class MortarSettings
{
	public GameObject target;
	public GameObject targetMarker;

	public float minSpeed;
	public float maxSpeed;
	public float minTargetOffset;
	public float maxTargetOffset;
	public float targetLead;

	public float arcHeight;
	public float speedIncrease;

	public bool usePredefinedTargetPos;
	public Transform[] predefinedTargetPos;
}
