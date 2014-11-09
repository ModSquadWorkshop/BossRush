using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	public bool useSpawner;
	public SpawnerSettings spawnerSettings;

	public bool useMortars;
	public int numMortars;
	public float launchInterval;

	public bool launchSpawners;
	public float spawnerLaunchInterval;

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

		if ( useMortars )
		{
			StartMortarLaunchAtInterval( numMortars, launchInterval );
		}

		if ( launchSpawners )
		{
			StartSpawnLaunchAtInterval( spawnerLaunchInterval );
		}
	}

	public virtual void OnDisable()
	{
		spawner.enabled = false;
		spawner.ResetSettings();
		CancelInvoke();
		StopAllCoroutines();
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

	public MortarAttack mortarLauncher
	{
		get
		{
			return spiderTank.mortarLauncher;
		}
	}

	public MortarAttack spawnerLauncher
	{
		get
		{
			return spiderTank.spawnerLauncher;
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

	public void StartMortarLaunchAtInterval( int mortarCount, float interval )
	{
		StartCoroutine( LaunchAtInterval( mortarLauncher, mortarCount, interval ) );
	}

	public void StartSpawnLaunchAtInterval( float interval )
	{
		StartCoroutine( LaunchAtInterval( spawnerLauncher, 1, interval ) );
	}

	private IEnumerator LaunchAtInterval( MortarAttack mortar, int mortarCount, float interval )
	{
		while ( true )
		{
			mortar.Launch( mortarCount );
			yield return new WaitForSeconds( interval );
		}
	}

	public void HealthTriggerCallback( HealthSystem health )
	{
		CancelInvoke();
		enabled = false;
		spiderTank.fleeState.enabled = true;
	}
}
