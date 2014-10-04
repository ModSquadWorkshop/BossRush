using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;
	public Gun mainCannon;
	public float turretSpeed;

	void Start()
	{
		player.gameObject.GetComponent<HealthSystem>().RegisterDeathCallback( PlayerDeath );
	}

	void Update()
	{
		// have main cannon track player with delay
		Quaternion look = Quaternion.LookRotation( player.position + Vector3.up * 2 - mainCannon.transform.position );
		mainCannon.transform.rotation = Quaternion.Lerp( mainCannon.transform.rotation, look, turretSpeed * Time.deltaTime );

		if ( !mainCannon.IsOnCooldown )
		{
			mainCannon.PerformPrimaryAttack();
		}
	}

	void PlayerDeath( HealthSystem playerHealth )
	{
		Destroy( GetComponent<EnemySpawner>() );
		Destroy( this );
	}
}
