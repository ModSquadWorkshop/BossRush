using UnityEngine;
using System.Collections;

public class Mortar : MonoBehaviour 
{
	private Vector3 _targetPos;
	private float _arcHeight;
	private float _time;
	private float _damage;

	public void Init( Vector3 startPos, Vector3 targetPos, float arcHeight, float time, float damage )
	{
		this.gameObject.transform.position = startPos;

		_targetPos = targetPos;
		_arcHeight = arcHeight;
		_time = time;
		_damage = damage;
	}

	void Start() 
	{
		iTween.MoveBy( this.gameObject, iTween.Hash( "y", +_arcHeight, "time", _time * 0.5f, "easeType", iTween.EaseType.easeOutQuad ) );
		iTween.MoveBy( this.gameObject, iTween.Hash( "y", -_arcHeight, "time", _time * 0.5f, "easeType", iTween.EaseType.easeInCubic ) );
		iTween.MoveAdd( this.gameObject, iTween.Hash( "position", _targetPos, "time", _time, "easeType", iTween.EaseType.linear ) );
	}
}