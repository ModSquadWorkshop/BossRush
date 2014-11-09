using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;

	public float levelOverDelay;

	private bool _levelOver; //< Used to prevent ResetLevel() from being called twice if both the player and the boss die.

	void Start()
	{
		_levelOver = false;

		player.GetComponent<DeathSystem>().RegisterDeathCallback( PlayerDied );
		boss.GetComponent<DeathSystem>().RegisterDeathCallback( BossDied );

		// hide the mouse
		Screen.showCursor = false;
	}

	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Application.Quit();
		}
	}

	public void PlayerDied( GameObject gameObjectz )
	{
		if ( !_levelOver )
		{
			_levelOver = true;
			Invoke( "ResetLevel", levelOverDelay );
		}
	}

	public void BossDied( GameObject gameObject )
	{
		if ( !_levelOver )
		{
			_levelOver = true;
			Invoke( "ResetLevel", levelOverDelay );
		}
	}

	void ResetLevel()
	{
		_levelOver = false;
		Application.LoadLevel( Application.loadedLevelName );
	}
}
