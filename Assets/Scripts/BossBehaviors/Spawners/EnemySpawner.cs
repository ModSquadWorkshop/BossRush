using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public delegate void EnemyCountChange( int count );

	public SpawnerSettings defaultSettings;

	public GameObject[] enemyTypes;
	public List<SpawnPoint> spawns;
	public int maxSpawnPoints;
	public int maxSpawned;

	public float delayBetweenNewEnemy = 0.1f;

	private bool _spawning = false; //!< Used to tell the coroutine to stop spawning.
	private int _enemyCount = 0; //!< A counter of the number of live minions in the world.

	private List<SpawnPoint> _availableSpawns;

	private SpawnerSettings _settings;
	private EnemyCountChange _enemyCountCallback = delegate( int count ) { }; //!< Callback used to notify listeners when the live enemy count changes.

	void Awake()
	{
		_settings = defaultSettings;
		_availableSpawns = new List<SpawnPoint>();

		foreach ( SpawnPoint spawn in spawns )
		{
			if ( spawn != null )
			{
				spawn.gameObject.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
			}
		}
	}

	void OnEnable()
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
		amount = Mathf.Min( amount, maxSpawned - _enemyCount );
		StartCoroutine( SpawnCoroutine( amount, enemyType ) );
	}

	private IEnumerator SpawnCoroutine( int amount, GameObject enemyType = null )
	{
		while ( amount > 0 && spawns.Count > 0 )
		{
			InitializeEnemyComponents( Instantiate( enemyType ?? enemyTypes[Random.Range( 0, enemyTypes.Length )] ) as GameObject );
			amount--;

			yield return new WaitForSeconds( delayBetweenNewEnemy );
		}
	}

	protected virtual void InitializeEnemyComponents( GameObject enemy )
	{
		SpawnPoint spawn = GetRandomAvailableSpawn();

		if ( spawn != null )
		{
			NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
			// disable the nav mesh agent to prevent a bug with the enemy spawning in the wrong location
			if ( agent != null )
			{
				agent.enabled = false;
			}

			// set the spawn point
			enemy.transform.position = spawn.spawnPoint.position;

			// move the enemy in a radius around the spawn point
			Vector2 radius = Random.insideUnitCircle * 10.0f;
			enemy.transform.Translate( radius.x, 0.0f, radius.y );

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

			//increment live enemy count
			enemyCount++;

			// re-enable the nav mesh agent
			if ( agent != null )
			{
				agent.enabled = true;
			}
		}
	}

	private SpawnPoint GetRandomAvailableSpawn()
	{
		_availableSpawns.Clear();

		foreach ( SpawnPoint spawnPoint in spawns )
		{
			if ( spawnPoint != null )
			{
				if ( spawnPoint.available )
				{
					_availableSpawns.Add( spawnPoint );
				}
			}
		}

		SpawnPoint spawn = null;

		int availableCount = _availableSpawns.Count;
		if ( availableCount > 0 )
		{
			int index = Random.Range( 0, availableCount );
			spawn = _availableSpawns[index];

			// this while loop is really to fix a number of bugs with null reference spawners
			// most of the time this while loop will never be triggered
			while ( spawn == null )
			{
				_availableSpawns.RemoveAt( index );
				availableCount = _availableSpawns.Count;

				if ( availableCount == 0 )
				{
					return null;
				}

				index++;
				if ( index >= availableCount )
				{
					index = 0;
				}

				spawn = _availableSpawns[index];
			}
		}

		return spawn;
	}

	public void ApplySettings( SpawnerSettings settings )
	{
		_settings = settings;
	}

	public void ResetSettings()
	{
		_settings = defaultSettings;
	}

	public void RegisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback += callback;
	}

	public void DeregisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback -= callback;
	}

	public void EnemyDeathCallback( GameObject enemy )
	{
		// decrement live enemy count
		enemyCount--;
	}

	public void AddSpawner( GameObject spawner )
	{
		SpawnPoint spawn = spawner.GetComponent<SpawnPoint>();
		if ( spawn == null )
		{
			spawn = spawner.AddComponent<SpawnPoint>();
		}

		spawns.Add( spawn );
		spawner.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
	}

	private void SpawnerDeathCallback( GameObject spawner )
	{
		SpawnPoint spawn = spawner.GetComponent<SpawnPoint>();
		if ( spawn != null )
		{
			spawn.available = true;
			spawns.Remove( spawn );
		}
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
}


[System.Serializable]
public class SpawnerSettings
{
	public int baseAmountPerWave;
	public int amountPerSpawner;
	public float waveInterval;
	public bool spawnOnStart;
}
