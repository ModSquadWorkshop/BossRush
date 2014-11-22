using UnityEngine;
using System.Collections;

public class PerkSpin : MonoBehaviour
{
	public Transform model;
	public float spin;
	private new Transform transform;

	void Awake()
	{
		transform = GetComponent<Transform>();
	}

	void Update()
	{
		transform.Rotate( Vector3.up, spin * Time.deltaTime, Space.Self );
	}
}
