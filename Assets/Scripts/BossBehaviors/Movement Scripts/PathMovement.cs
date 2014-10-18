using UnityEngine;
using System.Collections;

public class PathMovement : MoveTowardsTarget
{
	public delegate void DesinationReached( PathMovement movement );

	public Transform[] nodes;
	public bool traverseBackwards;

	private int _currentNode;
	private DesinationReached _destinationReachedCallback = delegate( PathMovement movement ) { };

	void OnEnable()
	{
		_currentNode = ( traverseBackwards ? nodes.Length - 1 : 0 );
		target = nodes[_currentNode];
	}

	public override void Update()
	{
		base.Update();

		if ( ( transform.position - nodes[_currentNode].position ).sqrMagnitude < 1.0f )
		{
			_currentNode = ( traverseBackwards ? _currentNode - 1 : _currentNode + 1 );
			if ( _currentNode < 0 || _currentNode >= nodes.Length )
			{
				_destinationReachedCallback( this );
			}
			else
			{
				target = nodes[_currentNode];
			}
		}
	}

	public void RegisterDestinationReachedCallback( DesinationReached callback )
	{
		_destinationReachedCallback += callback;
	}
}
