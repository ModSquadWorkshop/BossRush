using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoRing : MonoBehaviour
{
	static Quaternion ROTATION = Quaternion.Euler( 90.0f, 0.0f, 0.0f );

	public Image indicatorImage;

	private new Transform transform;

	private int _initialAmmo;
	private float _initialTime;
	private bool _isGun;
	private Gun _specialGun;
	private BeamWeapon _specialBeam;
	private WeaponSystem _playerWeapons;

	void Awake()
	{
		transform = GetComponent<Transform>();
	}

	void OnEnable()
	{
		//Get 3rd slot gun and total ammo, update with gun and current ammo
		_playerWeapons = GetComponentInParent<WeaponSystem>();
		_isGun = _playerWeapons.DetermineType();

		if ( _isGun )
		{
			_specialGun = _playerWeapons.weapons[2].GetComponent<Gun>();
			_initialAmmo = _specialGun.GetTotalAmmo();
			UpdateGunAmmoBar( _specialGun.GetTotalAmmo() );
			//Debug.Log( "GUN" );
		}
		else
		{
			_specialBeam = _playerWeapons.weapons[2].GetComponent<BeamWeapon>();
			_initialTime = _specialBeam.duration;
			UpdateBeamAmmoBar( _specialBeam.RunTime() );
			//Debug.Log( "BEAM" );
		}
	}

	void Update()
	{
		// this is done so that we can have the indicator ring
		// be a child of the object its attached to,
		// but not have it rotate with the parent object
		transform.rotation = ROTATION;

		if ( _isGun )
		{
			UpdateGunAmmoBar( _specialGun.GetTotalAmmo() );
		}
		else
		{
			UpdateBeamAmmoBar( _specialBeam.RunTime() );
		}
	}

	public void UpdateGunAmmoBar( int remainingAmmo )
	{
		float remainingPercent = (float)remainingAmmo / (float)_initialAmmo;
		//Debug.Log( remainingPercent );
		indicatorImage.fillAmount = remainingPercent;
		if ( remainingAmmo == 0 )
		{
			indicatorImage.fillAmount = 0;
			this.gameObject.SetActive( false );
		}
	}

	public void UpdateBeamAmmoBar( float runtime )
	{
		//Debug.Log( _initialTime - runtime );
		indicatorImage.fillAmount = ( _initialTime - runtime )/_initialTime;
		if ( _initialTime - runtime <= 0 )
		{
			indicatorImage.fillAmount = 0;
			this.gameObject.SetActive( false );
		}
	}
}
