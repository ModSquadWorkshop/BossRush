using UnityEngine;
using System.Collections;

public class KillParticleSystem : MonoBehaviour
{
	public float duration;

	void Start()
	{
		Invoke( "DestroySelf", duration );
	}

	void DestroySelf()
	{
		Destroy( gameObject );
	}
}
