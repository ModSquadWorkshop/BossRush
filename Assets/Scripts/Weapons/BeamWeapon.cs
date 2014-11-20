using UnityEngine;
using System;
using System.Collections;

public class BeamWeapon : Weapon
{
	public bool repeatDamage = true;
	public float maxRange;
	public float damageInterval;
	public int piercingsAmount;
	public float duration;
	public Transform impactParticles;

	private int _layerMask;
	private AudioSource _source;
	private LineRenderer beam;
	private Ray _ray;
	private Timer _beamTimer;
	private Timer _beamDuration;
	private DamageSystem _damageSystem;
	private Timer _damageTimer;
	private bool _damageDealt;
	private bool _done;

	void Awake()
	{
		beam = GetComponent<LineRenderer>();

		//raycast for everything but pickups
		_layerMask = 1 << LayerMask.NameToLayer( "Pickup" );
		_layerMask = ~_layerMask; // invert the mask

		_done = false;

		// the beam should only every have 2 vertexes
		// the width of the beam is the same from vertex to vertex
		beam.SetVertexCount( 2 );
		beam.enabled = false;

		// initialize the beam timers and the ray
		_damageTimer = new Timer( damageInterval, -1 );
		_beamTimer = new Timer( 0.1f, 1 );
		_beamDuration = new Timer( duration, 1 );
		_beamDuration.Start();
		_ray = new Ray();
		_source = GetComponent<AudioSource>();
		// get a reference to the damage system attached to the weapon
		_damageSystem = this.gameObject.GetComponent<DamageSystem>();

		// if a damage system doesn't exist one is created to avoid errors
		if ( _damageSystem == null )
		{
			_damageSystem = this.gameObject.AddComponent<DamageSystem>();
		}
	}

	void Update()
	{
		impactParticles.gameObject.SetActive( beam.enabled );
		if ( beam.enabled )
		{
			// the beam can only deal damage every time the timer is ticked at a set interval
			// every frame the beam is enabled, the timer is updated such that it's not relative to target collision
			_damageTimer.Update();

			// set the origin and direction of the ray to match the weapon/player's transformation
			_ray.origin = this.gameObject.transform.position;
			_ray.direction = this.gameObject.transform.forward;

			// set the starting vertex of the beam to the weapon/player position
			beam.SetPosition( 0, _ray.origin );

			// by default, the end vertex of the ray is the max forward distance from its origin
			Vector3 endVertex = _ray.origin + (_ray.direction * maxRange);

			// cast a ray and collect data on all of the objects it hits
			RaycastHit[] hits;
			hits = Physics.RaycastAll( _ray, maxRange, _layerMask );
			Array.Sort( hits, delegate( RaycastHit first, RaycastHit second )
			{
				return (int)( first.distance - second.distance );
			} );

			// loop through the hit targets and attempt to deal damage
			int length = Mathf.Min( hits.Length, piercingsAmount + 1 );
			for ( int i = 0; i < length; i++ )
			{
				RaycastHit hit = hits[i];

				// make sure the colliding object is one of the defined targets
				if ( _damageSystem.IsTarget( hit.collider.gameObject.tag ) )
				{
					// check to see if damage can be dealt according to the repeat flag
					if ( repeatDamage || !_damageDealt )
					{
						if ( _damageTimer.ticked )
						{
							// deal damage to the target
							_damageSystem.DamageObject( hit.collider.gameObject );
							_damageDealt = true;
						}
					}
				}
				endVertex = hit.point;
			}

			// set the end vertex of the beam according to raycast collisions and amount of piercings
			beam.SetPosition( 1, endVertex );
			impactParticles.position = endVertex;
			impactParticles.rotation = Quaternion.LookRotation( transform.position - endVertex );

			// the beam has a really short timer that automatically disables it when complete
			// the beam will quickly be stopped after the player stops "attacking"
			// if the player keeps "attacking", the timer is always reset, and the beam isn't disabled
			// this is necessary to avoid redundant checking of player input

			_beamTimer.Update();

			if ( _beamTimer.complete )
			{
				// disable the beam
				beam.enabled = false;
			}

			if ( _beamDuration.complete )
			{
				//Debug.Log( "BEAM OUT OF TIME" );
				_done = true;
			}
		}
		else
		{
			if ( _source != null && audio.isPlaying )
			{
				audio.Stop();
			}
		}
	}

	public bool IsDone()
	{
		return _done;
	}

	public void ResetTimer()
	{
		_beamDuration.Reset( true );
	}

	public float RunTime()
	{
		return _beamDuration.TimeRunning();
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

		if ( _source != null && !audio.isPlaying )
		{
			audio.Play();
		}

		_beamDuration.Update();
		_beamTimer.Reset( true );
	}
}
