using UnityEngine;
using System.Collections;

public class SpiderTankHealState : SpiderTankState
{
	public float healRate;

	public override void OnEnable()
	{
		base.OnEnable();

		shield.SetActive( true );
		shield.gameObject.GetComponent<DeathSystem>().RegisterDeathCallback( ShieldDestroyed );

		Physics.IgnoreCollision( collider, shield.collider, true );
	}

	public void Update()
	{
		spiderTank.health.Heal( healRate * Time.deltaTime );
		if ( spiderTank.health.atMaxHealth )
		{
			ShieldDestroyed( shield ); // make sure we go through a common exit path
		}
	}

	public override void OnDisable()
	{
		base.OnDisable();

		spiderTank.SetDamageBase();

		// on shutdown the shield gets destroyed before the spider tank,
		// so we have the potential for null references here
		if ( shield != null )
		{
			shield.SetActive( false );
			shield.gameObject.GetComponent<DeathSystem>().DeregisterDeathCallback( ShieldDestroyed );
		}
	}

	public void ShieldDestroyed( GameObject shield )
	{
		enabled = false;
		spiderTank.basicState.enabled = true;
	}
}
