using UnityEngine;
using System.Collections;
using XInputDotNetPure; //Might have trouble with visual studio


public class RumbleManager : MonoBehaviour 
{
	public bool rumble;
	public float leftForce;
	public float rightForce;
	public float rumbleTime;

	// Use this for initialization
	void Start() 
	{
		rumble = false;
	}

	IEnumerator Vibrate()
	{
		GamePad.SetVibration( 0,leftForce,rightForce );
		//Debug.Log( "VIBRATION BEGIN" );
		yield return new WaitForSeconds( rumbleTime );
		//Debug.Log( "VIBRATION END" );
		GamePad.SetVibration( 0,0f,0f );
	}

	public void Rumble()
	{
		if(rumble)
		{
			StartCoroutine( "Vibrate" );
			rumble = false;
		}
	}
}
