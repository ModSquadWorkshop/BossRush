using UnityEngine;
using System.Collections;

/*
// THIS CLASS HAS ONLY BEEN PARTIALLY IMPLEMENTED
*/

public class Level : MonoBehaviour 
{
	public string[] enemyTypes;
	public float enemySpawnInterval = 1.0f;
	public int enemiesPerSpawn = 1;

	private EnemyManager _enemyManager;

	void Start () 
	{
		_enemyManager = new EnemyManager( enemyTypes, enemySpawnInterval, enemiesPerSpawn );
		_enemyManager.StartSpawning();
	}

	void Update () 
	{
		// the EnemyManager is updated to advance the spawn timer
		_enemyManager.Update();
	}
}

/*
// THIS CLASS HAS ONLY BEEN PARTIALLY IMPLEMENTED
*/