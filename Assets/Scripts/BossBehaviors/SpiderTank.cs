using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;

	public Gun mainCannon;

	public float turretSpeed;

	void Update()
	{
		// have main cannon track player with delay
		Quaternion look = Quaternion.LookRotation( player.position - mainCannon.transform.position );
		mainCannon.transform.rotation = Quaternion.Lerp( mainCannon.transform.rotation, look, turretSpeed * Time.deltaTime );

		if ( !mainCannon.IsOnCooldown )
		{
			mainCannon.PerformPrimaryAttack();
		}
	}
}
