using UnityEngine;
using System.Collections;

public class SpiderTankState : MonoBehaviour
{
	public bool useWalkAnimation;
	public GlobalStateSettingsList globalSettings;

	[HideInInspector] public SpiderTank spiderTank;
	private GlobalStateSettings[] _stateSettings;

	public virtual void Awake()
	{
		spiderTank = GetComponent<SpiderTank>();
	}

	public virtual void OnEnable()
	{
		_stateSettings = new GlobalStateSettings[] { globalSettings.phaseOneSettings,
		                                             globalSettings.phaseTwoSettings,
		                                             globalSettings.phaseThreeSettings,
		                                             globalSettings.phaseFourSettings };

		if ( _stateSettings[spiderTank.currentPhase].useSpawner )
		{
			if ( spawner.spawners.Count > 0 )
			{
				spawner.enabled = true;
			}
			else
			{
				StartSpawnLaunchAtInterval( 2.0f );
			}
		}

		if ( _stateSettings[spiderTank.currentPhase].useMortars )
		{
			StartMortarLaunchAtInterval( spiderTank.phaseSettings[spiderTank.currentPhase].mortarSettings.amountOfMortars,
			                             spiderTank.phaseSettings[spiderTank.currentPhase].mortarSettings.launchInterval );
		}

		spiderTank.animator.SetBool( "Walk", useWalkAnimation );
	}

	public virtual void OnDisable()
	{
		spawner.enabled = false;

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

	public SpawnerMortarAttack spawnerLauncher
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


[System.Serializable]
public class GlobalStateSettings
{
	public bool walk;
	public bool useSpawner;
	public bool useMortars;
}

// this struct only exists to organize the settings in the inspector
[System.Serializable]
public class GlobalStateSettingsList
{
	public GlobalStateSettings phaseOneSettings;
	public GlobalStateSettings phaseTwoSettings;
	public GlobalStateSettings phaseThreeSettings;
	public GlobalStateSettings phaseFourSettings;
}
