using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;
	public GameObject mainMenu;
	public CameraFollow cameraFollow;
	public BossHealth bossHealthDisplay;

	public float startDelay;
	public float levelOverDelay;

	public bool skipMenu;

	private bool _levelOver; //!< Used to prevent ResetLevel() from being called twice if both the player and the boss die.

	void Start()
	{
		_levelOver = false;
		Screen.showCursor = true;

		player.GetComponent<DeathSystem>().RegisterDeathCallback( ImportantPeopleDied );
		boss.GetComponent<DeathSystem>().RegisterDeathCallback( BossDied );

		if ( skipMenu )
		{
			StartGame();
		}
	}

	void Update()
	{
		// check for exit request
		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Application.Quit();
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

	public void ShowBossHealthDisplay()
	{
		bossHealthDisplay.enabled = true;
	}

	void AwakeSpiderTank()
	{
		boss.SetActive( true );
	}

	void BossDied( GameObject bossObject )
	{
		ImportantPeopleDied( bossObject );

		// make player invincible then destroy all the enemies
		player.GetComponent<HealthSystem>().immune = true;

		foreach ( GameObject enemy in GameObject.FindGameObjectsWithTag( "Enemy" ) )
		{
			DeathSystem enemyDeath = enemy.GetComponent<DeathSystem>();
			if ( enemyDeath != null )
			{
				enemyDeath.KillDelayed( Random.Range( 0.5f, 1.5f ) );
			}
			else
			{
				Destroy( enemy );
			}
		}
	}

	void ImportantPeopleDied( GameObject importantPerson )
	{
		if ( !_levelOver )
		{
			_levelOver = true;
			cameraFollow.PullBackFromTarget( importantPerson );
			Invoke( "ResetLevel", levelOverDelay );
			bossHealthDisplay.enabled = false;
		}
	}

	void ResetLevel()
	{
		_levelOver = false;
		Application.LoadLevel( Application.loadedLevelName );
	}
}
