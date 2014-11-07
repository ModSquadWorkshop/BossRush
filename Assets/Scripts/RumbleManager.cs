using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class RumbleManager : MonoBehaviour
{
	[Range( 0.0f, 1f )]
	public float leftForce;
	[Range( 0.0f, 1f )]
	public float rightForce;

	public float rumbleTime;

	private float _force = 0.0f;
	private float _timeRemaining = 0.0f;
	private bool _rumbling = false;

	IEnumerator Vibrate()
	{
		_rumbling = true;
		do
		{
			float tempTime = _timeRemaining;
			_timeRemaining = 0.0f;
			GamePad.SetVibration( 0, leftForce * _force, rightForce * _force );
			yield return new WaitForSeconds( tempTime );
		}
		while ( _timeRemaining > 0.0f );

		KillRumble();
		_rumbling = false; ;
	}

	public void Rumble( float force )
	{
		_force = Mathf.Abs( force );
		_timeRemaining += _force * rumbleTime;
		if ( !_rumbling )
		{
			StartCoroutine( Vibrate() );
		}
	}

	public void Begin()
	{
		GamePad.SetVibration( 0, leftForce  * _force, rightForce * _force );
	}

	public void KillRumble()
	{
		GamePad.SetVibration( 0, 0f, 0f );
	}

	void OnDestroy()
	{
		KillRumble();
	}
}
