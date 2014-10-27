using UnityEngine;
using System.Collections;

public class FlankingSpawner : EnemySpawner
{
	public Transform[] controlPoints;

	[Range( 0.0f, 1.0f )]
	public float weight;

	protected override void InitializeEnemyComponents( GameObject enemy )
	{
		base.InitializeEnemyComponents( enemy );

		// add control point and weight
		MoveTowardsTargetFlanking enemyMovement = enemy.GetComponent<MoveTowardsTargetFlanking>();
		enemyMovement.controlPoint = controlPoints[_spawnIndex].position;
		enemyMovement.weight = weight;
	}
}
