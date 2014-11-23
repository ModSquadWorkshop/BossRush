using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	public float duration;
	public new Light light;

	void Start()
	{
		Invoke( "End", duration );
	}

	void Update()
	{
		light.range = Mathf.Lerp( light.range, 0, Time.deltaTime );
	}

	public void End()
	{
		Destroy( gameObject );
	}
}
