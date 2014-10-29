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
	}

	void Update () 
	{
		renderer.material.SetFloat( "_Cutoff", 1f - ( playerHealth.health / maxHealth) / 2f );
		//Debug.Log( 1f - ( playerHealth.health / maxHealth ) / 2f );
	}
}

