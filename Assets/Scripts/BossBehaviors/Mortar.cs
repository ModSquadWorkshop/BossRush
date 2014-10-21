using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour 
{
	public float damage;
	public float radius;

	private GameObject _player;
	private Vector3 _targetPos;
	private float _arcHeight;
	private float _time;

	private GameObject _targetMarker; // serves as a reference to a prefab
									  // unity throws an error when trying to destroy this
	private GameObject _marker; // the actual instantiated target marker

	public void Init( GameObject playerRef, Vector3 startPos, Vector3 targetPos, float arcHeight, float time, GameObject targetMarker )
	{
		this.gameObject.transform.position = startPos;

		_player = playerRef;
		_targetPos = targetPos;
		_arcHeight = arcHeight;
		_time = time;
		_targetMarker = targetMarker;
	}

	void Start() 
	{

		float halfTime = _time * 0.5f;

		iTween.MoveTo( this.gameObject, iTween.Hash( 
			"x", _targetPos.x, 
			"z", _targetPos.z, 
			"y", _targetPos.y + _arcHeight, 
			"time", halfTime, 
			"easeType", iTween.EaseType.linear, 
			"onComplete", "onMidComplete" ) );
	}

	void onMidComplete()
	{
		float halfTime = _time * 0.5f;

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
	}

	void onComplete()
	{
		if ( _marker != null )
		{
			Destroy( _marker );
		}

		if ( _player != null )
		{
			if ( Vector3.Distance( this.gameObject.transform.position, _player.transform.position) < radius )
			{
				HealthSystem healthSystem = _player.GetComponent<HealthSystem>();
				if ( healthSystem != null )
				{
					healthSystem.Damage( damage );
				}
			}
		}

		Destroy( this.gameObject );
	}
}