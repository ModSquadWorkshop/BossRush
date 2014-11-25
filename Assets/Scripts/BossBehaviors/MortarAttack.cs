using UnityEngine;
using System.Collections;

public class MortarAttack : MonoBehaviour
{
	public GameObject mortar;
	public AudioClip Launching;
	public MortarSettings mortarSettings;

	public float delayBetweenMortars = 0.15f;
	public bool allowMultipleLaunches;

	protected bool _firing = false;

	public void Launch( int numMortars )
	{
		if ( !_firing || allowMultipleLaunches )
		{
			StartCoroutine( LaunchMortar( numMortars ) );
			_firing = true;
			audio.clip = Launching;
			audio.Play();
			audio.volume = .7f;
			audio.priority = 0;
		}
	}

	protected virtual IEnumerator LaunchMortar( int numMortars )
	{
		while ( numMortars > 0 )
		{
			Mortar mortarObject = ( Instantiate( mortar ) as GameObject ).GetComponent<Mortar>();
			mortarObject.Init( mortarSettings, transform.position );
			numMortars--;

			yield return new WaitForSeconds( delayBetweenMortars );
		}

		_firing = false;
	}
}
