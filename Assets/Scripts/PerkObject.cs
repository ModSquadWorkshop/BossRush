using UnityEngine;
using System.Collections;

public class PerkObject : MonoBehaviour
{
	void OnCollisionEnter( Collision other )
	{
		if ( other.gameObject.tag == "Player" )
		{
			other.gameObject.GetComponent<PerkSystem>().AddPerk( this.gameObject.GetComponent<Perk>() );
			GetComponent<DeathSystem>().Kill();
		}
	}
}
