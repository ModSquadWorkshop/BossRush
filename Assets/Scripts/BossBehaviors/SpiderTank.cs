using UnityEngine;

public class SpiderTank : MonoBehaviour
{
	public Transform player;

	public Gun mainCanon;
	public BeamWeapon laserCanon;
	public Gun[] otherGuns;

	[HideInInspector] public SpiderTankBasicState basicState;
	[HideInInspector] public SpiderTankFleeState fleeState;
	[HideInInspector] public SpiderTankHealState healState;
	[HideInInspector] public SpiderTankLaserSpin laserSpin;
	[HideInInspector] public SpiderTankRushState rushState;
	[HideInInspector] public SpiderTankTurboState turboState;

	[HideInInspector] public EnemySpawner spawner;

	void Awake()
	{
		// retrieve all states
		basicState = GetComponent<SpiderTankBasicState>();
		fleeState = GetComponent<SpiderTankFleeState>();
		healState = GetComponent<SpiderTankHealState>();
		laserSpin = GetComponent<SpiderTankLaserSpin>();
		rushState = GetComponent<SpiderTankRushState>();
		turboState = GetComponent<SpiderTankTurboState>();

		spawner = GetComponent<EnemySpawner>();

		// register for player death callback
		player.gameObject.GetComponent<HealthSystem>().RegisterDeathCallback( PlayerDeath );
	}

	void PlayerDeath( HealthSystem playerHealth )
	{
		// okay
		// this is going to sound crazy, but...
		// if the player dies after the boss dies,
		// this callback still gets called, and then this is null
		// and the call to GetComponent<>() fails because of a null
		// reference error. So we check to see if this is null before
		// trying to destroy the spider tank.
		if ( this != null )
		{
			// gut all scripts to keep the boss in the scene but have it stop moving.
			Destroy( GetComponent<EnemySpawner>() );
			foreach ( SpiderTankState state in GetComponents<SpiderTankState>() )
			{
				Destroy( state );
			}
			Destroy( this );
		}
	}

	/**
	 * \brief Have the main canon look at the player gradually.
	 */
	public void LookMainCanon( float lookSpeed )
	{
		Quaternion look = Quaternion.LookRotation( player.position - mainCanon.transform.position );
		mainCanon.transform.rotation = Quaternion.Lerp( mainCanon.transform.rotation, look, lookSpeed * Time.deltaTime );
	}

	/**
	 * \brief Fire the main canon.
	 */
	public void FireMainCanon()
	{
		if ( !mainCanon.IsOnCooldown )
		{
			mainCanon.PerformPrimaryAttack();
		}
	}

	/**
	 * \brief Have all other guns look at the player gradually.
	 */
	public void LookOtherGuns( float lookSpeed )
	{
		foreach ( Gun gun in otherGuns )
		{
			Quaternion look = Quaternion.LookRotation( player.position - gun.transform.position );
			gun.transform.rotation = Quaternion.Lerp( gun.transform.rotation, look, lookSpeed * Time.deltaTime );
		}
	}

	/**
	 * \brief Have all other guns fire.
	 */
	public void FireOtherGuns()
	{
		foreach ( Gun gun in otherGuns )
		{
			if ( !gun.IsOnCooldown )
			{
				gun.PerformPrimaryAttack();
			}
		}
	}


	/**
	 * \brief Have the main canon and other guns look at the player gradually.
	 */
	public void LookAllGuns( float lookSpeed )
	{
		LookMainCanon( lookSpeed );
		LookOtherGuns( lookSpeed );
	}

	/**
	 * \brief Have the main canon and other guns fire.
	 */
	public void FireAllGuns()
	{
		FireMainCanon();
		FireOtherGuns();
	}
}
