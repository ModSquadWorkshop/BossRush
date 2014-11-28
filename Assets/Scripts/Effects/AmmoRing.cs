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
		// get special weapon and total ammo, update with gun and current ammo
		_playerWeapons = GetComponentInParent<WeaponSystem>();
		_isGun = _playerWeapons.DetermineSpecialType();

		if ( _isGun )
		{
			_specialGun = _playerWeapons.weapons[WeaponSystem.SPECIAL_WEAPON_SLOT].GetComponent<Gun>();
			_initialAmmo = _specialGun.ammo;

			UpdateGunAmmoBar( _specialGun.ammo );
		}
		else
		{
			_specialBeam = _playerWeapons.weapons[WeaponSystem.SPECIAL_WEAPON_SLOT].GetComponent<BeamWeapon>();
			_initialTime = _specialBeam.duration;

			UpdateBeamAmmoBar( _initialTime );
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
			UpdateGunAmmoBar( _specialGun.ammo );
		}
		else
		{
			UpdateBeamAmmoBar( _specialBeam.timeRemaining );
		}
	}

	public void UpdateGunAmmoBar( int remainingAmmo )
	{
		float remainingPercent = (float)remainingAmmo / (float)_initialAmmo;
		indicatorImage.fillAmount = remainingPercent;

		if ( remainingAmmo == 0 )
		{
			indicatorImage.fillAmount = 0;
			gameObject.SetActive( false );
		}
	}

	public void UpdateBeamAmmoBar( float runtime )
	{
		indicatorImage.fillAmount = runtime / _initialTime;

		if ( runtime <= 0 )
		{
			indicatorImage.fillAmount = 0;
			gameObject.SetActive( false );
		}
	}
}
