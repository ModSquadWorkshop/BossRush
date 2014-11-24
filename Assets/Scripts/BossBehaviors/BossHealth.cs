using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
	public GameObject boss;
	public GameObject bossMesh;
	public Image healthDisplay;

	private Canvas _healthbar;
	private MeshRenderer _renderer;

	void Start()
	{
		boss.GetComponent<HealthSystem>().RegisterHealthCallback( HealthCallback );
		_renderer = bossMesh.GetComponent<MeshRenderer>();
		_healthbar = GetComponent<Canvas>();
	}

	void Update()
	{
		if ( !_renderer.isVisible && boss.activeSelf )
		{
			_healthbar.enabled = true;
		}
		else
		{
			_healthbar.enabled = false;
		}
	}

	void HealthCallback( HealthSystem healthSystem, float healthChange )
	{
		healthDisplay.fillAmount = healthSystem.percent;
	}
}
