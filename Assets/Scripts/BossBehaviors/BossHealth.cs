using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour 
{
	private HealthSystem _health;
	private Scrollbar _scroll;
	public Canvas healthbar;
	
	// Use this for initialization
	void Start () 
	{
		_health = GetComponentInParent<HealthSystem>();
		_scroll = healthbar.GetComponentInChildren<Scrollbar>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		healthbar.enabled = true;
		_scroll.size = _health.percent;
		//Debug.Log( _scroll.size );
		Debug.Log( _health.percent );
		/*
		if ( !this.renderer.isVisible )
		{
			
		}
		else
		{
			//healthbar.enabled = false;
			healthbar.enabled = true;
			_scroll.size = _health.percent;
		}*/
	}
}
