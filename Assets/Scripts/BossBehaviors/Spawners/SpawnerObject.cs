using UnityEngine;
using System.Collections;

public class SpawnerObject : MonoBehaviour 
{
	public GameObject dirtEffect;

	[HideInInspector] public SpawnPoint spawnPoint;

	void Start()
	{
		Instantiate( dirtEffect, transform.position, Quaternion.identity );
	}
}
