using UnityEngine;

public class HealthRing : MonoBehaviour
{
	void Start()
	{
		HealthSystem playerHealth = GetComponentInParent<HealthSystem>();
		playerHealth.RegisterHealthCallback( UpdateHealthBar );
		UpdateHealthBar( playerHealth, 0.0f ); // set initial health bar state
	}

	public void UpdateHealthBar( HealthSystem playerHealth, float healthChange )
	{
		if ( playerHealth.health / playerHealth.maxHealth < 1 )
		{
			renderer.material.SetFloat( "_Cutoff", 1f - ( playerHealth.health / playerHealth.maxHealth ) );
		}
		else if ( playerHealth.health / playerHealth.maxHealth == 1 )
		{
			renderer.material.SetFloat( "_Cutoff", 0.0001f );
		}
	}
}

