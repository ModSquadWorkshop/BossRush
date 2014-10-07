using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	private SpiderTank _spiderTank;

	public SpiderTank spiderTank
	{
		get
		{
			return _spiderTank;
		}

		set
		{
			_spiderTank = value;
		}
	}
}
