using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	public bool useSpawner;
	public SpawnerSettings spawnerSettings;

	[HideInInspector] public SpiderTank spiderTank;

	public virtual void Awake()
	{
		spiderTank = GetComponent<SpiderTank>();
	}

	public virtual void OnEnable()
	{
		if ( useSpawner )
		{
			spawner.ApplySettings( spawnerSettings );
			spawner.enabled = true;
		}
	}

	public virtual void OnDisable()
	{
		spawner.enabled = false;
		spawner.ResetSettings();
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

	public BeamWeapon[] laserCanon
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

	public NavMeshAgent agent
	{
		get
		{
			return spiderTank.agent;
		}
	}

	public Collider doorCollider
	{
		get
		{
			return spiderTank.doorCollider;
		}
	}

	public void HealthTriggerCallback( HealthSystem health )
	{
		CancelInvoke();
		enabled = false;
		spiderTank.fleeState.enabled = true;
	}
}
