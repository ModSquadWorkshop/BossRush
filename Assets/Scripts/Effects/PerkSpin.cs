using UnityEngine;
using System.Collections;

public class PerkSpin : MonoBehaviour
{
	public Transform model;
	public float spin;
	public float lifetime;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
		Invoke( "End", lifetime );
	}

	void Update()
	{
		transform.Rotate( Vector3.up, spin * Time.deltaTime, Space.Self );
	}

	void End()
	{
		Destroy( this.gameObject );
	}
}
