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

	public float rumbleTime;

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
		GamePad.SetVibration( 0,0f,0f );
	}

	public void Rumble()
	{
		if(rumble)
		{
			//Debug.Log( rumble );
			StartCoroutine( "Vibrate" );
			rumble = false;
		}
	}

	public void Begin()
	{
		GamePad.SetVibration( 0, leftForce, rightForce );
	}

	public void Kill()
	{
		GamePad.SetVibration( 0, 0f, 0f );
	}
}
