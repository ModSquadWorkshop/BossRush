using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour 
{
	private HealthSystem _health;
	private Scrollbar _scroll;
	// Canvas healthbar;
	public GameObject bossMesh;
	public GameObject boss;
	private MeshRenderer _renderer;
	
	// Use this for initialization
	void Start () 
	{
		_health = boss.GetComponent<HealthSystem>();
		_health.RegisterHealthCallback( HealthCallback );
		_scroll = this.GetComponentInChildren<Scrollbar>();
		_renderer = bossMesh.GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( !_renderer.isVisible )
		{
			this.gameObject.SetActive( true );
			//Debug.Log( "BOSS NOT VISIBLE" );
		}
		else
		{
			this.gameObject.SetActive( false );
			//Debug.Log( "BOSS VISIBLE" );
		}
	}

	void HealthCallback( HealthSystem healthSystem, float healthChange )
	{
		_scroll.size = healthSystem.percent;
		//Debug.Log( _scroll.size );
	}
}
