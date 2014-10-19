using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public delegate void EnemyCountChange( int count );
	private int _enemyCount = 0; ///< A global counter of the number of live minions in the world.
	private EnemyCountChange _enemyCountCallback = delegate( int count ) { }; ///< Callback used to notify listeners when the live enemy count changes.

	public GameObject[] enemyTypes;
	public float[] enemySpawnChances;

	public Transform[] spawns;
	public float[] spawnPriorities;

	public int amountPerWave;
	public float waveInterval;
	public bool spawnOnStart;

	private Timer _spawnTimer;
	protected int _spawnIndex = 0; ///< Used for communication with derived spawners on which spawn index was used.

	public virtual void Start()
	{
		_spawnTimer = new Timer( waveInterval );

		if ( spawnOnStart )
		{
			StartSpawning();
		}
	}

	void Update()
	{
		_spawnTimer.Update();

		if ( _spawnTimer.IsTicked() )
		{
			Spawn();
		}
	}

	public void StartSpawning()
	{
		_spawnTimer.Start();
	}

	public void StopSpawning()
	{
		_spawnTimer.Stop();
	}

	/**
	 * \brief Spawns the default number of enemies.
	 */
	public void Spawn()
	{
		Spawn( amountPerWave );
	}

	/**
	 * \brief Spawns the specified number of enemies.
	 */
	public void Spawn( int amount )
	{
		for ( int i = 0; i < amount; i++ )
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			InitializeEnemyComponents( enemy );
		}
	}

	/**
	 * \brief Spawns the specified type of enemy.
	 */
	public void Spawn( GameObject type, int? amount = null )
	{
		// it's optional to provide the amount you want spawned
		// if you haven't provided an actual amount (e.g. anything greater 0),
		// then the EnemyManager spawns its normal amount (amountPerSpawn)
		if ( amount == null )
		{
			amount = amountPerWave;
		}

		for ( int i = 0; i < amount; i++ )
		{
			GameObject enemy = Instantiate( type ) as GameObject;
			InitializeEnemyComponents( enemy );
		}
	}

	protected virtual void InitializeEnemyComponents( GameObject enemy )
	{
		// set spawn point
		_spawnIndex = Random.Range( 0, spawns.Length );
		enemy.transform.position = spawns[_spawnIndex].position;

		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		MoveTowardsTarget moveTowards = enemy.GetComponent<MoveTowardsTarget>();
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
		enemyCount++;
	}

	protected GameObject GetEnemyBasedOnSpawnChance()
	{
		// TO DO
		return Instantiate( enemyTypes[0] ) as GameObject;
	}

	protected Vector3 GetSpawnBasedOnPriority()
	{
		// TO DO
		return spawns[0].position;
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

	public void RegisterEnemyCountCallback( EnemyCountChange callback )
	{
		_enemyCountCallback += callback;
	}
}
