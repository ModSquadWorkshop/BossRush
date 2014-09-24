using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	public GameObject player;
	public GameObject boss;

	void Start()
	{
		// register with health systems
		player.GetComponent<HealthSystem>().RegisterDeathCallback( new HealthSystem.DeathCallback( this.PlayerDied ) );
		boss.GetComponent<HealthSystem>().RegisterDeathCallback( new HealthSystem.DeathCallback( this.BossDied ) );
	}

	public void PlayerDied( HealthSystem playerHealth )
	{
		// TODO
		//Debug.Log( "Player Died" );
	}

	public void BossDied( HealthSystem bossHealth )
	{
		// TODO
		Debug.Log( "Boss Died" );
	}
}
