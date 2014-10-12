using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	public virtual void Awake()
	{
		spiderTank = GetComponent<SpiderTank>();
	}

	[HideInInspector] public SpiderTank spiderTank;
}
