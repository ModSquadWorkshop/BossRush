using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;

	public Text bossText;
	public Text playerText;

	public float levelOverDelay;

	private bool _levelOver; //< Used to prevent ResetLevel() from being called twice if both the player and the boss die.

	void Start()
	{
		_levelOver = false;

		// register with health systems
		HealthSystem playerHealth = player.GetComponent<HealthSystem>();
		playerHealth.RegisterHealthCallback( PlayerDamaged );
		playerText.text = "Player Health: " + playerHealth.health;
		player.GetComponent<DeathSystem>().RegisterDeathCallback( PlayerDied );

		HealthSystem bossHealth = boss.GetComponent<HealthSystem>();
		bossHealth.RegisterHealthCallback( BossDamaged );
		bossText.text = "Boss Health: " + bossHealth.health;
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

	public void PlayerDamaged( HealthSystem playerHealth, float damage )
	{
		playerText.text = "Player Health: " + playerHealth.health;
	}

	public void PlayerDied( GameObject gameObjectz )
	{
		if ( !_levelOver )
		{
			_levelOver = true;
			Invoke( "ResetLevel", levelOverDelay );
		}
	}

	public void BossDamaged( HealthSystem bossHealth, float damage )
	{
		bossText.text = "Boss Health: " + bossHealth.health;
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
