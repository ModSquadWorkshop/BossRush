using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;
	public Gun[] guns;
	public float turretSpeed;

	void Start()
	{
		player.gameObject.GetComponent<HealthSystem>().RegisterDeathCallback( PlayerDeath );
	}

	void Update()
	{
		// have main cannon track player with delay
		foreach ( Gun gun in guns )
		{
			Quaternion look = Quaternion.LookRotation( player.position - gun.transform.position );
			gun.transform.rotation = Quaternion.Lerp( gun.transform.rotation, look, turretSpeed * Time.deltaTime );

			if ( !gun.IsOnCooldown )
			{
				gun.PerformPrimaryAttack();
			}
		}
	}

	void PlayerDeath( HealthSystem playerHealth )
	{
		Destroy( GetComponent<EnemySpawner>() );
		Destroy( this );
	}
}
