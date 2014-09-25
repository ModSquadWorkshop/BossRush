using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject[] enemyTypes;
	public float[] enemySpawnChances;

	public Transform[] spawns;
	public float[] spawnPriorities;

	public int amountPerSpawn = 1;
	public float spawnInterval;
	public bool spawnOnStart = false;

	private Timer _spawnTimer;

	public virtual void Start()
	{
		_spawnTimer = new Timer( spawnInterval );

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

	// automatic spawn
	public virtual void Spawn()
	{
		for ( int i = 0; i < amountPerSpawn; i++ )
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = spawns[spawnIndex].position;

			InitializeEnemyComponents( enemy );
		}
	}

	// manual spawn
	public void Spawn( string type, int amount = -1 )
	{
		// it's optional to provide the amount you want spawned
		// if you haven't provided an actual amount (e.g. anything greater 0),
		// then the EnemyManager spawns its normal amount (amountPerSpawn)
		if ( amount <= 0 )
		{
			amount = amountPerSpawn;
		}

		for ( int i = 0; i < amount; i++ )
		{
			GameObject enemy = ( GameObject )Instantiate( Resources.Load( type ) );
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = spawns[spawnIndex].position;

			InitializeEnemyComponents( enemy );
		}
	}

	protected virtual void InitializeEnemyComponents( GameObject enemy )
	{
		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		MoveTowardsTarget moveTowards = ( MoveTowardsTarget )enemy.GetComponent( typeof( MoveTowardsTarget ) );
		if ( moveTowards != null )
		{
			moveTowards.target = GameObject.FindGameObjectWithTag( "Player" ).transform;
		}


		// likely more components to come later
		// ...
	}

	protected GameObject GetEnemyBasedOnSpawnChance()
	{
		// TO DO
		return ( GameObject )Instantiate( enemyTypes[0] );
	}

	protected Vector3 GetSpawnBasedOnPriority()
	{
		// TO DO
		return spawns[0].position;
	}
}