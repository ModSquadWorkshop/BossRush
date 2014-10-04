using UnityEngine;
using System.Collections;

public enum NodeMovement
{
	MOVE_TO_CLOSEST,
	MOVE_TO_FARTHEST,
	MOVE_TO_CLOSEST_TO_TARGET,
	MOVE_TO_FARTHEST_FROM_TARGET
}

public class MoveToNode : PhysicsMovement
{
	public Transform[] nodes;
	public NodeMovement movement;
	public Transform target;

	private Transform _targetNode;

	void Start()
	{
		switch ( movement )
		{
		case NodeMovement.MOVE_TO_CLOSEST:
			_targetNode = nodes[0];
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - transform.position ).sqrMagnitude < ( _targetNode.position - transform.position ).sqrMagnitude )
				{
					_targetNode = nodes[index];
				}
			}
			break;
		case NodeMovement.MOVE_TO_FARTHEST:
			_targetNode = nodes[0];
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - transform.position ).sqrMagnitude > ( _targetNode.position - transform.position ).sqrMagnitude )
				{
					_targetNode = nodes[index];
				}
			}
			break;
		case NodeMovement.MOVE_TO_CLOSEST_TO_TARGET:
			_targetNode = nodes[0];
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - target.position ).sqrMagnitude < ( _targetNode.position - target.position ).sqrMagnitude )
				{
					_targetNode = nodes[index];
				}
			}
			break;
		case NodeMovement.MOVE_TO_FARTHEST_FROM_TARGET:
			_targetNode = nodes[0];
			for ( int index = 1; index < nodes.Length; index++ )
			{
				if ( ( nodes[index].position - target.position ).sqrMagnitude > ( _targetNode.position - target.position ).sqrMagnitude )
				{
					_targetNode = nodes[index];
				}
			}
			break;
		}
	}

	void Update()
	{
		_movement = Vector3.Normalize( _targetNode.position - transform.position );
	}
}
