﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;
	public GameObject mainMenu;
	public CameraFollow cameraFollow;

	public float startDelay;
	public float levelOverDelay;

	public bool skipMenu;

	private bool _levelOver; //< Used to prevent ResetLevel() from being called twice if both the player and the boss die.

	void Start()
	{
		_levelOver = false;

		player.GetComponent<DeathSystem>().RegisterDeathCallback( PlayerDied );
		boss.GetComponent<DeathSystem>().RegisterDeathCallback( BossDied );

		if ( skipMenu )
		{
			StartGame();
		}
	}

	public void StartGame()
	{
		player.SetActive( true );
		mainMenu.SetActive( false );
		cameraFollow.enabled = true;

		// hide the mouse
		Screen.showCursor = false;


		Invoke( "AwakeSpiderTank", startDelay );
	}

	void AwakeSpiderTank()
	{
		boss.SetActive( true );
	}

	void PlayerDied( GameObject gameObjectz )
	{
		if ( !_levelOver )
		{
			_levelOver = true;
			Invoke( "ResetLevel", levelOverDelay );
		}
	}

	void BossDied( GameObject gameObject )
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
