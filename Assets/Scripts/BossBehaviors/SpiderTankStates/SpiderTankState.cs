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
	}

	public Gun mainCanon
	{
		get
		{
			return spiderTank.mainCanon;
		}
	}

	public BeamWeapon laserCanon
	{
		get
		{
			return spiderTank.laserCanon;
		}
	}

	public Gun[] otherGuns
	{
		get
		{
			return spiderTank.otherGuns;
		}
	}

	public EnemySpawner spawner
	{
		get
		{
			return spiderTank.spawner;
		}
	}

	public GameObject shield
	{
		get
		{
			return spiderTank.shield;
		}
	}

	public void HealthTriggerCallback( HealthSystem health )
	{
		CancelInvoke();
		enabled = false;
		spiderTank.healState.enabled = true;
	}
}
