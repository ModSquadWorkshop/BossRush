using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	public float duration;
	public float decay;

	private float _radius;

	void Start()
	{
		Invoke( "End", duration );
	}

	void Update()
	{
		radius -= decay * Time.deltaTime;
	}

	public void End()
	{
		Destroy( gameObject );
	}

	public float radius
	{
		get
		{
			return _radius;
		}

		set
		{
			_radius = value;
			transform.localScale = new Vector3( _radius * 2, _radius * 2, _radius * 2 );
		}
	}
}
