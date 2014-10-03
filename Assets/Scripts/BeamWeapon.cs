using UnityEngine;
using System.Collections;

public class BeamWeapon : Weapon 
{
    public bool repeatDamage = true;

    public float maxRange = 20.0f;
    public LineRenderer beam;
    public float beamWidth = 0.5f;

    private Ray _ray;
    private RaycastHit _hit;
    private Timer _beamTimer;

	void Start() 
    {
        beam = GetComponent<LineRenderer>();
        beam.SetVertexCount( 2 );
        beam.SetWidth( beamWidth, beamWidth );

        _beamTimer = new Timer( 0.1f, 1 );

        base.Start();
	}
	
	public override void Update() 
    {
        if ( _beamTimer.IsRunning() )
        {
            _ray = Camera.main.ScreenPointToRay( Input.mousePosition );

            beam.SetPosition( 0, this.transform.position );

            if ( Physics.Raycast( _ray, out _hit, maxRange ) )
            {
                beam.SetPosition( 1, _hit.point + _hit.normal );
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
