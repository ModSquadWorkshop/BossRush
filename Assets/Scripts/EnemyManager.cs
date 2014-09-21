using UnityEngine;
using System.Collections;

sealed public class EnemyManager 
{
	public GameObject minion;

	public int _amountPerSpawn;

	private string[] _enemyTypes;
	private float _spawnDelay;
	private Timer _spawnTimer;

	public EnemyManager ( string[] enemyTypes, float spawnDelay, int amountPerSpawn = 1 )
	{
		_enemyTypes = enemyTypes;
		_amountPerSpawn = amountPerSpawn;
		_spawnDelay = spawnDelay;
		_spawnTimer = new Timer( _spawnDelay );
	}

	public void Update () 
	{
		_spawnTimer.Update();

		if ( _spawnTimer.IsTicked() ) 
		{
			Spawn();
		}
	}

	public void StartSpawning () 
	{
		_spawnTimer.Start();
	}

	public void StopSpawning () 
	{
		_spawnTimer.Stop();
	}

	public void Spawn () 
	{
		GameObject[] spawns = GetSpawnPoints();

		for ( int i = 0; i < _amountPerSpawn; i++ ) 
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = (Vector3)spawns[spawnIndex].transform.position;

			InitializeEnemyComponents( enemy );
		}
	}

	public void Spawn ( string type, int amount = -1 ) 
	{
		// it's optional to provide the amount you want spawned
		// if you haven't provided an actual amount (e.g. anything greater 0),
		// then the EnemyManager spawns its normal amount (_amountPerSpawn)
		if ( amount <= 0 ) 
		{
			amount = _amountPerSpawn;
		}

		GameObject[] spawns = GetSpawnPoints();
		
		for ( int i = 0; i < amount; i++ ) 
		{
			GameObject enemy = (GameObject)Level.Instantiate( Resources.Load( type ) );
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = (Vector3)spawns[spawnIndex].transform.position;

			InitializeEnemyComponents( enemy );
		}
	}

	private void InitializeEnemyComponents ( GameObject enemy ) 
	{
		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		MoveTowardsTarget follow = (MoveTowardsTarget)enemy.GetComponent( typeof(MoveTowardsTarget) );
		if ( follow != null ) 
		{
			follow.target = GameObject.FindGameObjectWithTag( "Player" );
		}


		// likely more components to come later
		// ...
	}

	private GameObject[] GetSpawnPoints () 
	{
		return GameObject.FindGameObjectsWithTag( "Spawn" );
	}

	private GameObject GetEnemyBasedOnSpawnChance () 
	{
		// TO DO
		return (GameObject)Level.Instantiate( minion );
	}
}