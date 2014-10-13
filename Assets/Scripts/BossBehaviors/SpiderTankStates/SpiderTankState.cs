using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	[HideInInspector] public SpiderTank spiderTank;

	public virtual void Awake()
	{
		spiderTank = GetComponent<SpiderTank>();
	}
}
