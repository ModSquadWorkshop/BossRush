using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour 
{
	public float damage;
	public float radius;

	private float		_duration;
	private GameObject  _target;
	private Vector3		_targetPos;
	private GameObject  _targetMarker; // serves as a reference to a prefab
									  // unity throws an error when trying to destroy this
	private GameObject  _marker; // the actual instantiated target marker

	private const float ARC_HEIGHT = 100.0f;

	public void Init( float duration, Vector3 startPos, GameObject target, Vector3 targetPos, GameObject targetMarker )
	{
		this.gameObject.transform.position = startPos;

		_duration = duration;
		_target = target;
		_targetPos = targetPos;
		_targetMarker = targetMarker;
	}

	void Start() 
	{

		float halfTime = _duration * 0.5f;

		iTween.MoveTo( this.gameObject, iTween.Hash( 
			"x", _targetPos.x, 
			"z", _targetPos.z,
			"y", _targetPos.y + ARC_HEIGHT, 
			"time", halfTime, 
			"easeType", iTween.EaseType.linear, 
			"onComplete", "onMidComplete" ) );
	}

	void onMidComplete()
	{
		float halfTime = _duration * 0.5f;

		iTween.MoveTo( this.gameObject, iTween.Hash(
			"y", _targetPos.y,
			"time", halfTime,
			"delay", halfTime,
			"easeType", iTween.EaseType.linear,
			"oncomplete", "onComplete" ) );

		if ( _targetMarker != null )
		{
			_marker = Instantiate( _targetMarker ) as GameObject;
			_marker.transform.position = _targetPos;
		}

		// flip the mortar so it faces down
		this.transform.localScale.Scale( Vector3.one * -1.0f );
	}

	void onComplete()
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

		Destroy( this.gameObject );
	}
}