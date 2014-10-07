using UnityEngine;
using System.Collections;

public class BeamWeapon : Weapon
{
	public bool repeatDamage = true;

	public LineRenderer beam;
	public float beamWidth;
	public float maxRange;
	public float damageInterval;

	private Ray _ray;
	private RaycastHit _rayHit;
	private Timer _beamTimer;

	private DamageSystem _damageSystem;
	private Timer _damageTimer;
	private bool _damageDealt;

	public override void Start()
	{
		if ( beam == null )
		{
			beam = this.gameObject.AddComponent<LineRenderer>();
		}

		// the beam should only every have 2 vertexes
		// the width of the beam is the same from vertex to vertex
		beam.SetVertexCount( 2 );
		beam.SetWidth( beamWidth, beamWidth );
		beam.enabled = false;

		// initialize the beam timers and the ray
		_damageTimer = new Timer( damageInterval, -1 );
		_beamTimer = new Timer( 0.1f, 1 );
		_ray = new Ray();

		// get a reference to the damage system attached to the weapon
		_damageSystem = this.gameObject.GetComponent<DamageSystem>();

		// if a damage system doesn't exist one is created to avoid errors
		if ( _damageSystem == null )
		{
			_damageSystem = this.gameObject.AddComponent<DamageSystem>();
		}

		base.Start();
	}

	public override void Update() 
	{
		if ( beam.enabled )
		{
			// the beam can only deal damage every time the timer is ticked at a set interval
			// every frame the beam is enabled, the timer is updated such that it's not relative to target collision
			_damageTimer.Update();

			// set the origin and direction of the ray to match the weapon/player's transformation
			_ray.origin = this.transform.position;
			_ray.direction = this.transform.forward;

			// set the starting vertex of the beam to the weapon/player position
			beam.SetPosition( 0, this.transform.position );

			// cast a ray to check for collision detection
			if ( Physics.Raycast( _ray, out _rayHit, maxRange ) )
			{
				// make sure the colliding object is one of the defined targets
				if ( _damageSystem.IsTarget( _rayHit.collider.gameObject.tag ) )
				{
					// check to see if damage can be dealt according to the repeat flag
					if ( repeatDamage || !_damageDealt )
					{
						if ( _damageTimer.IsTicked() )
						{
							// deal damage to the target
							_damageSystem.DamageObject( _rayHit.collider.gameObject );
							_damageDealt = true;
						}
					}

					// set the end vertex of the beam to the target position
					beam.SetPosition( 1, _rayHit.point + _rayHit.normal );
				}
				else
				{
					// set the end vertex of the beam to the maxRange from the starting vertex
					beam.SetPosition( 1, this.transform.position + ( _ray.direction * maxRange ) );
				}
			}

			// the beam has a really short timer that automatically disables it when complete
			// the beam will quickly be stopped after the player stops "attacking"
			// if the player keeps "attacking", the timer is always reset, and the beam isn't disabled
			// this is necessary to avoid redundant checking of player input

			_beamTimer.Update();

			if ( _beamTimer.IsComplete() )
			{
				// disable the beam
				beam.enabled = false;
			}
		}

		base.Update();
	}

	public override void PerformPrimaryAttack()
	{
		// reset the beam and enable it
		if ( !beam.enabled )
		{
			_damageTimer.Reset( true );
			_damageDealt = false;

			beam.enabled = true;
		}

		_beamTimer.Reset( true );
	}
}
