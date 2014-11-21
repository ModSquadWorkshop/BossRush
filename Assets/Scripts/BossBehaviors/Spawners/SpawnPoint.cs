using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
	public bool available = true;
	[HideInInspector] public Transform spawnPoint;

	void Awake()
	{
		if ( spawnPoint == null )
		{
			spawnPoint = GetComponent<Transform>();
		}
	}

}
