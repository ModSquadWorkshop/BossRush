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

		// configure all the things
		_speed = Random.Range( _settings.minSpeed, _settings.maxSpeed );
		_targetPos = new Vector3( startPos.x, _settings.arcHeight, startPos.z );
		_speed = Random.Range( _settings.minSpeed, _settings.maxSpeed );
		_velocity = Vector3.Normalize( _targetPos - transform.position ) * _speed;

		// calculate time to apex
		float timeToApex = Vector3.Distance( transform.position, _targetPos ) / _speed;
		Invoke( "OnMidComplete", timeToApex );

		transform.rotation = Quaternion.LookRotation( Vector3.up );
	}

	void Update()
	{
		this.gameObject.transform.Translate( _velocity * Time.deltaTime, Space.World );
	}

	void OnMidComplete()
	{
		// calculate the things
		_speed *= _settings.speedIncrease;
		transform.rotation = Quaternion.LookRotation( Vector3.down );
		_targetPos = GetTargetPosition();
		transform.position = _targetPos + new Vector3( 0.0f, _settings.arcHeight, 0.0f );
		_marker = Instantiate( _settings.targetMarker, _targetPos, Quaternion.identity ) as GameObject;
		_velocity = Vector3.Normalize( _targetPos - transform.position ) * _speed;

		// calculate time to target
		float timeToTarget = Vector3.Distance( transform.position, _targetPos ) / _speed;
		Invoke( "OnComplete", timeToTarget );
	}

	void OnComplete()
	{
		GetComponent<DeathSystem>().Kill();
		Destroy( _marker );
	}

	private Vector3 GetTargetPosition()
	{
		Vector3 offset = Random.insideUnitCircle * Random.Range( _settings.minTargetOffset, _settings.maxTargetOffset );
		offset.z = offset.y; // Random.insideUnitCircle returns a 2D vector with (x, y), so we swap y with z for an accurate 3D position
		offset.y = 0.0f;

		Transform target = _settings.targets[Random.Range( 0, _settings.targets.Length )];

		// just give up if the target has already been destroyed
		if ( target == null )
		{
			Destroy( gameObject );
			return new Vector3();
		}

		// take the amount to lead the target by into account
		if ( target.rigidbody != null )
		{
			offset += Vector3.Normalize( target.rigidbody.velocity ) * _settings.targetLead;
		}

		return offset + target.position;
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
	public Transform[] targets;
	public GameObject targetMarker;

	public float minSpeed;
	public float maxSpeed;
	public float minTargetOffset;
	public float maxTargetOffset;
	public float targetLead;

	public float arcHeight;
	public float speedIncrease;
}
