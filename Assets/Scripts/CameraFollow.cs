using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform followTarget;
	public float offset;
	public float followSpeed = 1.0f;

	void Update()
	{
		transform.position = Vector3.Lerp( transform.position, followTarget.position + new Vector3( 0.0f, offset, 0.0f ), followSpeed * Time.deltaTime );
	}
}