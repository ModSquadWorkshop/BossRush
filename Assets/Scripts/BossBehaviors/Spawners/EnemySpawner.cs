using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public delegate void EnemyCountChange( int count );

	public SpawnerSettings settings;

	public GameObject[] enemyTypes;
	public List<GameObject> spawners;

	public float delayBetweenNewEnemy;

	private bool _spawning; //!< Used to tell the coroutine to stop spawning.
	private int _enemyCount; //!< A counter of the number of live minions in the world.

	private EnemyCountChange _enemyCountCallback = delegate( int count ) { }; //!< Callback used to notify listeners when the live enemy count changes.

	void Awake()
	{
		_spawning = false;
		_enemyCount = 0;

		foreach ( GameObject spawner in spawners )
		{
			spawner.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
		}
	}

	void OnEnable()
	{
		StartCoroutine( StartSpawning() );
	}

	public IEnumerator StartSpawning()
	{
		_spawning = true;

		while ( _spawning )
		{
			Spawn();
			yield return new WaitForSeconds( settings.waveInterval );
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
		Spawn( settings.baseAmountPerWave + ( settings.amountPerSpawner * spawners.Count ) );
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
		amount = Mathf.Min( amount, settings.maxSpawned - _enemyCount );
		StartCoroutine( SpawnCoroutine( amount, enemyType ) );
	}

	private IEnumerator SpawnCoroutine( int amount, GameObject enemyType = null )
	{
		while ( amount > 0 && spawners.Count > 0 && _enemyCount < settings.maxSpawned )
		{
			InitializeEnemyComponents( Instantiate( enemyType ?? enemyTypes[Random.Range( 0, enemyTypes.Length )] ) as GameObject );
			amount--;

			yield return new WaitForSeconds( delayBetweenNewEnemy );
		}
	}

	private void InitializeEnemyComponents( GameObject enemy )
	{
		Transform spawn = GetRandomSpawner();

		if ( spawn != null )
		{
			NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
			// disable the nav mesh agent to prevent a bug with the enemy spawning in the wrong location
			if ( agent != null )
			{
				agent.enabled = false;
			}

			// set the spawn point
			enemy.transform.position = spawn.position;

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

	public void RegisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback += callback;
	}

	public void DeregisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback -= callback;
	}

	public void AddSpawner( GameObject spawner, SpawnPoint spawnPoint )
	{
		spawner.GetComponent<SpawnerObject>().spawnPoint = spawnPoint;
		spawner.GetComponent<DeathSystem>().RegisterDeathCallback( SpawnerDeathCallback );
		spawners.Add( spawner );
	}

	private void SpawnerDeathCallback( GameObject spawner )
	{
		SpawnerObject spawnerObject = spawner.GetComponent<SpawnerObject>();

		if ( spawnerObject.spawnPoint != null )
		{
			spawnerObject.spawnPoint.available = true;
			spawnerObject.spawnPoint = null;
		}

		spawners.Remove( spawner );
	}

	private void EnemyDeathCallback( GameObject enemy )
	{
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

	private Transform GetRandomSpawner()
	{
		Transform spawn = null;

		int availableCount = spawners.Count;
		if ( availableCount > 0 )
		{
			int index = Random.Range( 0, availableCount );
			spawn = spawners[index].transform;

			// this while loop is really to fix a number of bugs with null reference spawners
			// most of the time this while loop will never be triggered
			while ( spawn == null )
			{
				spawners.RemoveAt( index );
				availableCount = spawners.Count;

				if ( availableCount == 0 )
				{
					return null;
				}

				index++;
				if ( index >= availableCount )
				{
					index = 0;
				}

				spawn = spawners[index].transform;
			}
		}

		return spawn;
	}
}


[System.Serializable]
public class SpawnerSettings
{
	public int baseAmountPerWave;
	public int amountPerSpawner;
	public float waveInterval;

	public int maxSpawnPoints;
	public int maxSpawned;
}
