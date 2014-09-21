using UnityEngine;
using System.Collections;

sealed public class EnemySpawner : MonoBehaviour 
{
	public GameObject[] enemyTypes;
	public float[] enemySpawnChances;

	public Vector3[] spawns;
	public float[] spawnPriorities;

	public int amountPerSpawn = 1;
	public float spawnInterval;
	public bool spawnOnStart = false;

	private Timer _spawnTimer;

	void Start() 
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
	public void Spawn() 
	{
		for ( int i = 0; i < amountPerSpawn; i++ ) 
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = (Vector3)spawns[spawnIndex];

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
			GameObject enemy = (GameObject)Instantiate( Resources.Load( type ) );
			int spawnIndex = Random.Range( 0, spawns.Length );

			enemy.transform.position = (Vector3)spawns[spawnIndex];

			InitializeEnemyComponents( enemy );
		}
	}

	private void InitializeEnemyComponents( GameObject enemy ) 
	{
		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		MoveTowardsTarget moveTowards = (MoveTowardsTarget)enemy.GetComponent( typeof( MoveTowardsTarget ) );
		if ( moveTowards != null ) 
		{
			moveTowards.target = GameObject.FindGameObjectWithTag( "Player" );
		}


		// likely more components to come later
		// ...
	}

	private GameObject GetEnemyBasedOnSpawnChance() 
	{
		// TO DO
		return (GameObject)Instantiate( enemyTypes[0] );
	}

	private Vector3 GetSpawnBasedOnPriority() 
	{
		// TO DO
		return (Vector3)spawns[0];
	}
}