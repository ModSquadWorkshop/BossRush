using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;
	public GameObject mainMenu;
	public GameObject pauseMenu;
	public CameraFollow cameraFollow;
	public BossHealth bossHealthDisplay;

	public float startDelay;
	public float levelOverDelay;

	public bool skipMenu;

	private bool _paused = false;
	private bool _levelOver = false; //!< Used to prevent ResetLevel() from being called twice if both the player and the boss die.

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
			if ( !_paused )
			{
				PauseGame();
			}
			else
			{
				UnpauseGame();
			}
		}
	}

	public void StartGame()
	{
		player.SetActive( true );
		mainMenu.SetActive( false );
		cameraFollow.enabled = true;

		// hide the mouse
		Screen.showCursor = false;

		enabled = true;
		Invoke( "AwakeSpiderTank", startDelay );
	}

	public void PauseGame()
	{
		if ( !_paused )
		{
			// pause the game by freezing time and notifying all objects that they should pause
			Time.timeScale = 0.0f;
			foreach ( GameObject gameObject in GameObject.FindObjectsOfType<GameObject>() )
			{
				gameObject.SendMessage( "OnPause", SendMessageOptions.DontRequireReceiver );
			}
			Screen.showCursor = true;
			pauseMenu.SetActive( true );
			_paused = true;
		}
	}

	public void UnpauseGame()
	{
		if ( _paused )
		{
			Time.timeScale = 1.0f;
			foreach ( GameObject gameObject in GameObject.FindObjectsOfType<GameObject>() )
			{
				gameObject.SendMessage( "OnUnpause", SendMessageOptions.DontRequireReceiver );
			}
			Screen.showCursor = false;
			pauseMenu.SetActive( false );
			_paused = false;
		}
	}

	public void QuitGame()
	{
		Application.Quit();
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
