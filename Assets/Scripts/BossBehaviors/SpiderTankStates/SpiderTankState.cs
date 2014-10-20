using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	[HideInInspector] public SpiderTank spiderTank;

	public virtual void Awake()
	{
		spiderTank = GetComponent<SpiderTank>();
	}

	public Transform player
	{
		get
		{
			return spiderTank.player;
		}

		set
		{
			spiderTank.player = value;
		}
	}

	public Gun mainCanon
	{
		get
		{
			return spiderTank.mainCanon;
		}

		set
		{
			spiderTank.mainCanon = value;
		}
	}

	public BeamWeapon laserCanon
	{
		get
		{
			return spiderTank.laserCanon;
		}

		set
		{
			spiderTank.laserCanon = value;
		}
	}

	public Gun[] otherGuns
	{
		get
		{
			return spiderTank.otherGuns;
		}

		set
		{
			spiderTank.otherGuns = value;
		}
	}

	public void HealthTriggerCallback( HealthSystem health )
	{
		CancelInvoke();
		enabled = false;
		spiderTank.fleeState.returnState = spiderTank.healState;
		spiderTank.fleeState.enabled = true;
	}
}
