using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public float shakeForce;
	public float shakeTime;

	public void Shake( float amount )
	{
		amount = Mathf.Abs( amount );

		// using a hashtable is necessary because
		// we need to set the "islocal" value
		Hashtable shakeSettings = new Hashtable();
		shakeSettings.Add( "amount", Vector3.left * shakeForce * amount );
		shakeSettings.Add( "time", shakeTime * amount );
		shakeSettings.Add( "islocal", true );
		iTween.ShakePosition( Camera.main.gameObject, shakeSettings );
	}
}
