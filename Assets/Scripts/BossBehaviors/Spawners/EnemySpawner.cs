using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public delegate void EnemyCountChange( int count );

	public GameObject[] enemyTypes;
	public List<Transform> spawns;
	public int maxSpawnPoints;

	public SpawnerSettings defaultSettings;
	public int maxSpawned;

	protected int _spawnIndex = 0; //!< Used for communication with derived spawners on which spawn index was used.

	private bool _spawning = false; //!< Used to tell the coroutine to stop spawning.
	private int _enemyCount = 0; //!< A counter of the number of live minions in the world.
	private SpawnerSettings _settings;
	private EnemyCountChange _enemyCountCallback = delegate( int count ) { }; //!< Callback used to notify listeners when the live enemy count changes.

	public void Awake()
	{
		_settings = defaultSettings;

		foreach ( Transform spawnPoint in spawns )
		{
			spawnPoint.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
		}
	}

	public void OnEnable()
	{
		if ( _settings.spawnOnStart )
		{
			StartCoroutine( StartSpawning() );
		}
	}

	public IEnumerator StartSpawning()
	{
		_spawning = true;
		while ( _spawning )
		{
			Spawn();
			yield return new WaitForSeconds( _settings.waveInterval );
		}
	}

	public void StopSpawning()
	{
		_spawning = false;
	}

	/**
	 * \brief Spawns the default number of enemies.
	 */
	public void Spawn()
	{
		Spawn( _settings.baseAmountPerWave + ( _settings.amountPerSpawner * spawns.Count ) );
	}

	/**
	 * \brief Spawns the specified number of enemies.
	 * 
	 * \param enemyType An optional parameter to specify the type of enemy to be spawned.
	 * If not provided, the type of each enemy will be picked at random from the spawner's
	 * list of default enemies.
	 */
	public void Spawn( int amount, GameObject enemyType = null )
	{
		if ( spawns.Count > 0 )
		{
			for ( int i = 0; i < amount && _enemyCount < maxSpawned; i++ )
			{
				InitializeEnemyComponents( Instantiate( enemyType ?? enemyTypes[Random.Range( 0, enemyTypes.Length )] ) as GameObject );
			}
		}
	}

	protected virtual void InitializeEnemyComponents( GameObject enemy )
	{
		// set spawn point
		_spawnIndex = Random.Range( 0, spawns.Count );
		enemy.transform.position = spawns[_spawnIndex].position;

		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		ITargetBasedMovement moveTowards = enemy.GetComponent( typeof( ITargetBasedMovement ) ) as ITargetBasedMovement;
		if ( moveTowards != null )
		{
			moveTowards.target = GameObject.FindGameObjectWithTag( "Player" ).transform;
		}

		// register for death notification
		DeathSystem enemyDeath = enemy.GetComponent<DeathSystem>();
		if ( enemyDeath != null )
		{
			enemyDeath.RegisterDeathCallback( EnemyDeathCallback );
		}

		// increment live enemy count
		enemyCount = enemyCount + 1;
	}

	public void EnemyDeathCallback( GameObject enemy )
	{
		// decrement live enemy count
		enemyCount--;
	}

	public int enemyCount
	{
		get
		{
			return _enemyCount;
		}

		set
		{
			_enemyCount = value;
			_enemyCountCallback( _enemyCount );
		}
	}

	public void ApplySettings( SpawnerSettings settings )
	{
		_settings = settings;
	}

	public void ResetSettings()
	{
		_settings = defaultSettings;
	}

	public void AddSpawnPoint( Transform spawnPoint )
	{
		spawns.Add( spawnPoint );
		spawnPoint.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
	}

	public void RegisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback += callback;
	}

	public void DeregisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback -= callback;
	}

	private void SpawnerDeathCallback( GameObject spawner )
	{
		spawns.Remove( spawner.transform );
	}
}

[System.Serializable]
public class SpawnerSettings
{
	public int baseAmountPerWave;
	public int amountPerSpawner;
	public float waveInterval;
	public bool spawnOnStart;
}
