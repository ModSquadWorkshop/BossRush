using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IndicatorRing : MonoBehaviour
{
	static Quaternion ROTATION = Quaternion.Euler( 90.0f, 0.0f, 0.0f );

	public Image indicatorImage;

	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
	}

	void Start()
	{
		HealthSystem playerHealth = GetComponentInParent<HealthSystem>();
		playerHealth.RegisterHealthCallback( UpdateHealthBar );
		UpdateHealthBar( playerHealth, 0.0f ); // set initial health bar state
	}

	void Update()
	{
		// this is done so that we can have the indicator ring
		// be a child of the object its attached to,
		// but not have it rotate with the parent object
		transform.localRotation = ROTATION;
	}

	public void UpdateHealthBar( HealthSystem healthSystem, float healthChange )
	{
		indicatorImage.fillAmount = healthSystem.percent;
	}
}
