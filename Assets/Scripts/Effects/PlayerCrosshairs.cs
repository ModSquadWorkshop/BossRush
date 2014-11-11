using UnityEngine;
using System.Collections;

public class PlayerCrosshairs : MonoBehaviour
{
	public MeshRenderer[] renderers; //!< Because I was too lazy to do proper sprites.

	public bool show
	{
		set
		{
			foreach ( MeshRenderer renderer in renderers )
			{
				renderer.enabled = value;
			}
		}

		get
		{
			return renderers[0].enabled;
		}
	}
}
