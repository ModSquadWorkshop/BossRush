using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour 
{
	private Vector3 _targetPos;
	private float _arcHeight;
	private float _time;
	private float _damage;

	private GameObject _targetMarker; // serves as a reference to a prefab
									  // unity throws an error when trying to destroy this
	private GameObject _marker; // the actual instantiated target marker

	public void Init( Vector3 startPos, Vector3 targetPos, float arcHeight, float time, float damage, GameObject targetMarker )
	{
		this.gameObject.transform.position = startPos;

		_targetPos = targetPos;
		_arcHeight = arcHeight;
		_time = time;
		_damage = damage;
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
			"y", _targetPos.y - _arcHeight,
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

		Destroy( this.gameObject );
	}
}