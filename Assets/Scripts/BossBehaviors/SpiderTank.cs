using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;

	public Gun mainCanon;
	public Gun[] otherGuns;

	public SpiderTankState initialState;

	public float timeToCrazy;

	private SpiderTankState[] _states;
	private int _currentState = 0;

	void Start()
	{
		player.gameObject.GetComponent<HealthSystem>().RegisterDeathCallback( PlayerDeath );

		// get states
		_states = gameObject.GetComponents<SpiderTankState>();
		for ( int index = 0; index < _states.Length; index++ )
		{
			_states[index].spiderTank = this;
			_states[index].enabled = false;

			// check starting state
			if ( _states[index] == initialState )
			{
				_currentState = index;
			}
		}

		// enable initial state
		_states[_currentState].enabled = true;

		Invoke( "NextState", timeToCrazy );
	}

	void PlayerDeath( HealthSystem playerHealth )
	{
		// gut all scripts to avoid null references
		Destroy( GetComponent<EnemySpawner>() );
		foreach ( SpiderTankState state in _states )
		{
			Destroy( state );
		}
		Destroy( this );
	}

	private void NextState()
	{
		_states[_currentState].enabled = false;
		_currentState = ++_currentState % _states.Length;
		_states[_currentState].enabled = true;
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
