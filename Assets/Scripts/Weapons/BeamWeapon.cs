using UnityEngine;
using System;
using System.Collections;

public class BeamWeapon : Weapon
{
	[Tooltip( "The maximum distance to be used for the raycast. Make sure this is long engough that the end of the laser doesn't appear on screen." )]
	public float maxRange;
	[Tooltip( "The amount of time the beam can be fired before stopping. Equivalent to ammo in a Gun." )]
	public float duration;
	public Transform impactParticles;
	public LayerMask layerMask;

	private AudioSource _audio;
	private LineRenderer _beam;
	private Ray _ray;
	private float _beamDuration;
	private DamageSystem _damageSystem;
	private bool _continueFiring; //!< Used to enable some trickery to ensure the weapon fires continuously.

	void Awake()
	{
		_beam = GetComponent<LineRenderer>();

		// the beam should only ever have 2 vertexes
		_beam.SetVertexCount( 2 );

		// initialize the beam ray
		_ray = new Ray();
		_audio = GetComponent<AudioSource>();

		// get a reference to the damage system attached to the weapon
		_damageSystem = GetComponent<DamageSystem>();

		_beamDuration = duration;
		_continueFiring = false;
	}

	void OnEnable()
	{
		impactParticles.gameObject.SetActive( true );
		_beam.enabled = true;

		if ( _audio != null )
		{
			audio.Play();
		}
	}

	void Update()
	{
		// set the origin and direction of the ray to match the weapon/player's transformation
		_ray.origin = transform.position;
		_ray.direction = transform.forward;

		// set the starting vertex of the beam to the weapon/player position
		_beam.SetPosition( 0, _ray.origin );

		// by default, the end vertex of the ray is the max forward distance from its origin
		Vector3 endVertex = _ray.origin + ( _ray.direction * maxRange );

		// cast a ray and collect data on all of the objects it hits
		RaycastHit[] hits;
		hits = Physics.RaycastAll( _ray, maxRange, layerMask );

		// sort the hits from nearest to farthest
		Array.Sort( hits, delegate( RaycastHit first, RaycastHit second )
		{
			return (int)( first.distance - second.distance );
		} );

		if ( hits.Length > 0 )
		{
			RaycastHit hit = hits[0];

			// make sure the colliding object is one of the defined targets
			if ( _damageSystem.IsTarget( hit.collider.gameObject.tag ) )
			{
				_damageSystem.DamageObject( hit.collider.gameObject );
			}

			endVertex = hit.point;
		}

		// set the end vertex of the beam according to raycast collisions and amount of piercings
		_beam.SetPosition( 1, endVertex );
		impactParticles.position = endVertex;
		impactParticles.rotation = Quaternion.LookRotation( transform.position - endVertex );

		// update the remaining time for the beam
		_beamDuration -= Time.deltaTime;
		_continueFiring = false;
	}

	void LateUpdate()
	{
		if ( !_continueFiring )
		{
			enabled = false;
		}
	}

	void OnDisable()
	{
		impactParticles.gameObject.SetActive( false );
		_beam.enabled = false;

		if ( _audio != null && audio.isPlaying )
		{
			audio.Stop();
		}
	}

	public bool isDone
	{
		get
		{
			return _beamDuration <= 0.0f;
		}
	}

	public void ResetTimer()
	{
		_beamDuration = duration;
	}

	public float timeRemaining
	{
		get
		{
			return _beamDuration;
		}
	}

	public override void PerformPrimaryAttack()
	{
		// turn on the beam
		enabled = true;
		_continueFiring = true;
	}
}
