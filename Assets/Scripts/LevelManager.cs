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
		playerHealth.RegisterDamageCallback( new HealthSystem.DamageCallback( this.PlayerDamaged ) );
		playerHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( this.PlayerDied ) );
		playerText.text = "Player Health: " + playerHealth.health;

		HealthSystem bossHealth = boss.GetComponent<HealthSystem>();
		bossHealth.RegisterDamageCallback( new HealthSystem.DamageCallback( this.BossDamaged ) );
		bossHealth.RegisterDeathCallback( new HealthSystem.DeathCallback( this.BossDied ) );
		bossText.text = "Boss Health: " + bossHealth.health;
	}

	public void PlayerDamaged( HealthSystem playerHealth, float damage )
	{
		playerText.text = "Player Health: " + playerHealth.health;
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

	public void BossDamaged( HealthSystem bossHealth, float damage )
	{
		bossText.text = "Boss Health: " + bossHealth.health;
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
