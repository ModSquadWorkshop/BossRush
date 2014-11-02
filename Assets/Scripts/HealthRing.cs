using UnityEngine;

public class HealthRing : MonoBehaviour
{
	static Quaternion ROTATION = Quaternion.Euler( 90.0f, 0.0f, 0.0f );

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
		// this allows the health ring to be childed to the object,
		// but to not rotate as the object rotates.
		transform.rotation = ROTATION;
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

