using UnityEngine;
using System.Collections;

public class HealthRing : MonoBehaviour 
{
	HealthSystem playerHealth;
	float maxHealth;

	void Start () 
	{
		playerHealth = this.gameObject.GetComponentInParent<HealthSystem>();
		maxHealth = this.gameObject.GetComponentInParent<HealthSystem>().maxHealth;
		renderer.material.SetFloat( "_Cutoff", 0.0001f );
	}

	public void UpdateHealthBar () 
	{
		if ( playerHealth.health / maxHealth < 1 )
		{
			renderer.material.SetFloat( "_Cutoff", 1f - (playerHealth.health / maxHealth) );/// 2f );
			//Debug.Log( 1f - ( playerHealth.health / maxHealth ) / 2f );
		}
		else if ( playerHealth.health / maxHealth == 1 )
		{
			renderer.material.SetFloat( "_Cutoff", 0.0001f );
		}
	}
}

