using UnityEngine;
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

		// register with health systems
		player.GetComponent<HealthSystem>().RegisterDeathCallback( new HealthSystem.DeathCallback( this.PlayerDied ) );
		boss.GetComponent<HealthSystem>().RegisterDeathCallback( new HealthSystem.DeathCallback( this.BossDied ) );
	}

	public void PlayerDied( HealthSystem playerHealth )
	{
		// TODO say that the player loses

		if ( !_levelOver )
		{
			_levelOver = true;
			Invoke( "ResetLevel", levelOverDelay );
		}
	}

	public void BossDied( HealthSystem bossHealth )
	{
		// TODO say that player wins

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
