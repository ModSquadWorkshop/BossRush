using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;

	public Gun mainCanon;
	public Gun[] otherGuns;

	public float stateChangeInterval;

	public SpiderTankState[] states;
	private int _currentState = 0;

	void Awake()
	{
		player.gameObject.GetComponent<HealthSystem>().RegisterDeathCallback( PlayerDeath );

		// enable initial state
		states[_currentState].enabled = true;

		Invoke( "NextState", stateChangeInterval );
	}

	void PlayerDeath( HealthSystem playerHealth )
	{
		// okay
		// this is going to sound crazy, but...
		// if the player dies after the boss dies,
		// this callback still gets called, and then this is null
		// and the call to GetComponent<>() fails because of a null
		// reference error. So we check to see if this is null before
		// trying to destroy the spider tank.
		if ( this != null )
		{
			// gut all scripts to keep the boss in the scene but have it stop moving.
			Destroy( GetComponent<EnemySpawner>() );
			foreach ( SpiderTankState state in states )
			{
				Destroy( state );
			}
			Destroy( this );
		}
	}

	private void NextState()
	{
		states[_currentState].enabled = false;
		_currentState = ++_currentState % states.Length;
		states[_currentState].enabled = true;

		Invoke( "NextState", stateChangeInterval );
	}

	public void LookMainCanon( float lookSpeed )
	{
		Quaternion look = Quaternion.LookRotation( player.position - mainCanon.transform.position );
		mainCanon.transform.rotation = Quaternion.Lerp( mainCanon.transform.rotation, look, lookSpeed * Time.deltaTime );
	}

	public void FireMainCanon()
	{
		if ( !mainCanon.IsOnCooldown )
		{
			mainCanon.PerformPrimaryAttack();
		}
	}

	public void LookOtherGuns( float lookSpeed )
	{
		foreach ( Gun gun in otherGuns )
		{
			Quaternion look = Quaternion.LookRotation( player.position - gun.transform.position );
			gun.transform.rotation = Quaternion.Lerp( gun.transform.rotation, look, lookSpeed * Time.deltaTime );
		}
	}

	public void FireOtherGuns()
	{
		foreach ( Gun gun in otherGuns )
		{
			if ( !gun.IsOnCooldown )
			{
				gun.PerformPrimaryAttack();
			}
		}
	}

	public void LookAllGuns( float lookSpeed )
	{
		LookMainCanon( lookSpeed );
		LookOtherGuns( lookSpeed );
	}

	public void FireAllGuns()
	{
		FireMainCanon();
		FireOtherGuns();
	}
}
