using UnityEngine;
using System.Collections;

sealed public class MoveTowardsTarget : MonoBehaviour 
{
	public GameObject target;
	public float speed = 10.0f;
	
	void Update () 
	{
		this.transform.position = Vector3.MoveTowards( this.transform.position, target.transform.position, Time.deltaTime * speed );
	}
}