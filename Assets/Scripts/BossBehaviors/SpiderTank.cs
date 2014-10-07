using UnityEngine;
using System.Collections;

public class SpiderTank : MonoBehaviour
{
	public Transform player;
	public Gun mainCannon;
	public float turretSpeed;

	public SpiderTankState initialState;

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
}
