using UnityEngine;
using System.Collections;

public class PathMovement : MoveTowardsTarget
{
	public Transform[] nodes;
	public bool traverseBackwards;

	private int _currentNode;

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
				enabled = false;

				// each time the boss traverses the path it will then
				// have to walk it the other way, so reverse the
				// direction each time.
				traverseBackwards = !traverseBackwards;
			}
			else
			{
				target = nodes[_currentNode];
			}
		}
	}
}
