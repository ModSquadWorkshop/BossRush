using UnityEngine;
using System.Collections;

public class BeamWeapon : Weapon
{
	public bool repeatDamage = true;

	public LineRenderer beam;
	public float beamWidth;
	public float maxRange;

	private Ray _ray;
	private RaycastHit _hit;
	private Timer _beamTimer;
	private DamageSystem _damageSystem;

	public override void Start()
	{
		beam = this.gameObject.GetComponent<LineRenderer>();
		beam.SetVertexCount( 2 );
		beam.SetWidth( beamWidth, beamWidth );

		_beamTimer = new Timer( 0.1f, 1 );
		_ray = new Ray();

		_damageSystem = this.gameObject.GetComponent<DamageSystem>();

		if ( _damageSystem == null )
		{
			_damageSystem = this.gameObject.AddComponent<DamageSystem>();
		}

		base.Start();
	}

	public override void Update() 
	{
		if ( _beamTimer.IsRunning() )
		{
			_ray.origin = this.transform.position;
			_ray.direction = this.transform.forward * maxRange;

			beam.SetPosition( 0, this.transform.position );

			if ( Physics.Raycast( _ray, out _hit, maxRange ) )
			{
				if ( _damageSystem.IsTarget( _hit.collider.gameObject.tag ) )
				{
					_damageSystem.DamageObject( _hit.collider.gameObject );

					beam.SetPosition( 1, _hit.point + _hit.normal );
				}
				else
				{
					beam.SetPosition( 1, this.transform.position + ( _ray.direction * maxRange ) );
				}
			}

			_beamTimer.Update();

			if ( _beamTimer.IsComplete() )
			{
				beam.enabled = false;
			}
		}

		base.Update();
	}

	public override void PerformPrimaryAttack()
	{
		beam.enabled = true;

		_beamTimer.Reset( true );
	}
}
