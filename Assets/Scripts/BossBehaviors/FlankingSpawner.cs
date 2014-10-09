using UnityEngine;
using System.Collections;

public class FlankingSpawner : EnemySpawner
{
	public Transform[] controlPoints;

	[Range( 0.0f, 1.0f )]
	public float weight;

	public int _currentControlPoint = 0;

	protected override void InitializeEnemyComponents( GameObject enemy )
	{
		enemy.transform.position = spawns[_currentControlPoint].position;

		// add control point and weight
		MoveTowardsTargetFlanking enemyMovement = enemy.GetComponent<MoveTowardsTargetFlanking>();
		enemyMovement.target = GameObject.FindGameObjectWithTag( "Player" ).transform; // *sigh* I guess this is fine.
		enemyMovement.controlPoint = controlPoints[_currentControlPoint].position;
		enemyMovement.weight = weight;

		_currentControlPoint = ++_currentControlPoint % controlPoints.Length;
	}
}
