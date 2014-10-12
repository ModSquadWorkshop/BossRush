using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject[] enemyTypes;
	public float[] enemySpawnChances;

	public Transform[] spawns;
	public float[] spawnPriorities;

	public int amountPerWave;
	public float waveInterval;
	public bool spawnOnStart;

	private Timer _spawnTimer;

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

	// automatic spawn
	public void Spawn()
	{
		for ( int i = 0; i < amountPerWave; i++ )
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			InitializeEnemyComponents( enemy );
		}
	}

	// manual spawn
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
		int spawnIndex = Random.Range( 0, spawns.Length );
		enemy.transform.position = spawns[spawnIndex].position;

		// if the enemy uses a MoveTowardsTarget script, the target needs to be set
		MoveTowardsTarget moveTowards = enemy.GetComponent<MoveTowardsTarget>();
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
		return Instantiate( enemyTypes[0] ) as GameObject;
	}

	protected Vector3 GetSpawnBasedOnPriority()
	{
		// TO DO
		return spawns[0].position;
	}
}