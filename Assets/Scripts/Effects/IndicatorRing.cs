using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IndicatorRing : MonoBehaviour
{
	static Quaternion ROTATION = Quaternion.Euler( 90.0f, 0.0f, 0.0f );

	public Image indicatorImage;

	void Start()
	{
		HealthSystem playerHealth = GetComponentInParent<HealthSystem>();
		playerHealth.RegisterHealthCallback( UpdateHealthBar );
		UpdateHealthBar( playerHealth, 0.0f ); // set initial health bar state
	}

	public void UpdateHealthBar( HealthSystem healthSystem, float healthChange )
	{
		indicatorImage.fillAmount = healthSystem.percent;
	}
}
