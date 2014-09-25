using UnityEngine;
using System.Collections;

public class FlankingSpawner : EnemySpawner
{
	public Transform[] controlPoints;

	public float weight;

	public int _currentControlPoint = 0;

	public override void Spawn()
	{
		for ( int i = 0; i < amountPerSpawn; i++ )
		{
			GameObject enemy = GetEnemyBasedOnSpawnChance();
			InitializeEnemyComponents( enemy );
		}
	}

	protected override void InitializeEnemyComponents( GameObject enemy )
	{
		enemy.transform.position = spawns[_currentControlPoint].position;

		// add control point and weight
		MoveTowardsTargetFlanking enemyMovement = enemy.GetComponent<MoveTowardsTargetFlanking>();
		enemyMovement.target = GameObject.FindGameObjectWithTag( "Player" ).transform; // *sigh* I guess this is fine.
		enemyMovement.controlPoint = controlPoints[_currentControlPoint];
		enemyMovement.weight = weight;

		_currentControlPoint = ++_currentControlPoint % controlPoints.Length;
	}
}
