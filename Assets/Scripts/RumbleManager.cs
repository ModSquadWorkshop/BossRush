using UnityEngine;
using System.Collections;
using XInputDotNetPure; //Might have trouble with visual studio


public class RumbleManager : MonoBehaviour
{
	public bool rumble;

	[Range( 0.0f, 1f )]
	public float leftForce;
	[Range( 0.0f, 1f )]
	public float rightForce;

	public float rumbleMod;
	public float rumbleTime;

	private float _force;

	// Use this for initialization
	void Start()
	{
		rumble = false;
	}

	IEnumerator Vibrate()
	{
		GamePad.SetVibration( 0, leftForce, rightForce );
		//Debug.Log( "VIBRATION BEGIN" );
		yield return new WaitForSeconds( rumbleTime );
		//Debug.Log( "VIBRATION END" );
		GamePad.SetVibration( 0, 0f, 0f );
	}

	public void Rumble( float force )
	{
		if( rumble )
		{
			_force = Mathf.Abs(force);
			StartCoroutine( "Vibrate" );
			rumble = false;
			//Debug.Log( rightForce + (rumbleMod * _force) );

		}
	}

	public void Begin()
	{
		GamePad.SetVibration( 0, leftForce + ( rumbleMod * _force ), rightForce + ( rumbleMod * _force ) );
	}

	public void Kill()
	{
		GamePad.SetVibration( 0, 0f, 0f );
	}

	void OnDestroy()
	{
		Kill();
	}
	 
}
