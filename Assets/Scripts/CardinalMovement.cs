using UnityEngine;
using System.Collections;

sealed public class CardinalMovement : MonoBehaviour 
{
	public float baseSpeed = 200.0f;
	public float speedMultiplier = 1.0f;

	private float _speed;

	void Start () 
	{
		_speed = baseSpeed * speedMultiplier;
	}

	void Update () 
	{
		Vector3 position = this.transform.position;

		position.x = this.transform.position.x + Input.GetAxis( "Horizontal" ) * Time.deltaTime * _speed * speedMultiplier;
		position.z = this.transform.position.z + Input.GetAxis( "Vertical" )   * Time.deltaTime * _speed * speedMultiplier;

		this.transform.position = position;
	}

	public float speed 
	{
		get { return _speed * speedMultiplier; }
		set { _speed = value; }
	}
}