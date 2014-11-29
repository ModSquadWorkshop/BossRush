using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Menu : MonoBehaviour
{
	public Button firstSelected;

	void OnEnable()
	{
		GameObject.FindObjectOfType<EventSystem>().SetSelectedGameObject( firstSelected.gameObject );
		firstSelected.Select();
	}
}
