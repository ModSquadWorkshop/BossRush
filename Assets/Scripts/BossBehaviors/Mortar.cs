using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour 
{
	public float damage;
	public float radius;

	private float		_speed;
	private GameObject  _target;
	private Vector3		_targetPos;
	private GameObject  _targetMarker; // serves as a reference to a prefab
									  // unity throws an error when trying to destroy this
	private GameObject  _marker; // the actual instantiated target marker
	private Vector3		_velocity;
	private bool		_peakReached;

	private const float ARC_HEIGHT = 100.0f;
	private const float SPEED_INCREASE = 2.5f;

	public void Init( float speed, Vector3 startPos, GameObject target, Vector3 targetPos, GameObject targetMarker )
	{
		this.gameObject.transform.position = startPos;

		_speed = speed;
		_target = target;
		_targetPos = targetPos;
		_targetMarker = targetMarker;
	}

	void Start() 
	{
		_peakReached = false;
		_velocity = Vector3.zero;
		_targetPos.Set( _targetPos.x, _targetPos.y + ARC_HEIGHT, _targetPos.z );
	}

	void Update()
	{
		if ( _peakReached )
		{
			this.gameObject.transform.Translate( _velocity * Time.deltaTime, Space.World );

			if ( Vector3.Distance( this.gameObject.transform.position, _targetPos ) < 2.0f )
			{
				OnComplete();
			}
		}
		else
		{
			this.gameObject.transform.position = Vector3.SmoothDamp( this.gameObject.transform.position, _targetPos, ref _velocity, Time.deltaTime * _speed );
			this.gameObject.transform.rotation = Quaternion.AngleAxis( Mathf.Atan2( _velocity.y, _velocity.x ) * Mathf.Rad2Deg, Vector3.forward * 90.0f );

			if ( Vector3.Distance( this.gameObject.transform.position, _targetPos ) < 2.0f )
			{
				OnMidComplete();
			}
		}
	}

	void OnMidComplete()
	{
		_speed *= SPEED_INCREASE;
		_targetPos.Set( _targetPos.x, _targetPos.y - ARC_HEIGHT, _targetPos.z );
		_velocity = Vector3.down * _speed;

		this.gameObject.transform.rotation = Quaternion.AngleAxis( Mathf.Atan2( _velocity.y, _velocity.x ) * Mathf.Rad2Deg, Vector3.forward * 90.0f );

		if ( _targetMarker != null )
		{
			_marker = Instantiate( _targetMarker ) as GameObject;
			_marker.transform.position = _targetPos;
		}

		_peakReached = true;
	}

	void OnComplete()
	{
		if ( _marker != null )
		{
			Destroy( _marker );
		}

		if ( _target != null )
		{
			float distance = Vector3.Distance( this.gameObject.transform.position, _target.transform.position );
			if ( distance < radius )
			{
				HealthSystem healthSystem = _target.GetComponent<HealthSystem>();
				if ( healthSystem != null )
				{
					float damageDropoff = damage * (distance / radius);
					healthSystem.Damage( damage - damageDropoff );
				}
			}
		}

		this.enabled = false;
		Destroy( this.gameObject );
	}
}