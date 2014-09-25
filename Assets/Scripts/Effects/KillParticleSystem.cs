using UnityEngine;
using System.Collections;

public class KillParticleSystem : MonoBehaviour
{
	void Update ()
	{
		if ( !particleSystem.IsAlive() )
		{
			Destroy( gameObject );
		}
	}
}
