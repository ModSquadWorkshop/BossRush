using UnityEngine;
using System.Collections;

public enum NodeMovementTypes
{
	MOVE_TO_CLOSEST,
	MOVE_TO_FARTHEST,
	MOVE_TO_CLOSEST_TO_TARGET,
	MOVE_TO_FARTHEST_FROM_TARGET
}

public class MoveToNode : MoveTowardsTarget
{
	public Transform[] nodes;
	public NodeMovementTypes movement;
	public Transform referenceTarget;

	public float resetInterval;

	void Start()
	{
		DeathSystem targetDeath = referenceTarget.gameObject.GetComponent<DeathSystem>();
		if ( targetDeath != null )
		{
			targetDeath.RegisterDeathCallback( TargetDeathCallback );
		}
	}

	public void OnEnable()
	{
		Transform tempTarget = nodes[0];

		switch ( movement )
		{
		case NodeMovementTypes.MOVE_TO_CLOSEST:

			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - transform.position ).sqrMagnitude < ( tempTarget.position - transform.position ).sqrMagnitude )
				{
					target = nodes[index];
				}
			}
			break;
		case NodeMovementTypes.MOVE_TO_FARTHEST:
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - transform.position ).sqrMagnitude > ( tempTarget.position - transform.position ).sqrMagnitude )
				{
					target = nodes[index];
				}
			}
			break;
		case NodeMovementTypes.MOVE_TO_CLOSEST_TO_TARGET:
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - referenceTarget.position ).sqrMagnitude < ( tempTarget.position - referenceTarget.position ).sqrMagnitude )
				{
					target = nodes[index];
				}
			}
			break;
		case NodeMovementTypes.MOVE_TO_FARTHEST_FROM_TARGET:
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - referenceTarget.position ).sqrMagnitude > ( tempTarget.position - referenceTarget.position ).sqrMagnitude )
				{
					target = nodes[index];
				}
			}
			break;
		}

		target = tempTarget;

		if ( resetInterval > 0.0f )
		{
			Invoke( "OnEnable", resetInterval );
		}
	}
}
