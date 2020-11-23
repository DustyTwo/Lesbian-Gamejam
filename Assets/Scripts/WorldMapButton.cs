using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WorldMapButton : MonoBehaviour, IPointerDownHandler
{
	public SceneIndexes scene;
	private WorldMapHandler parent;

	public void Awake()
	{
		parent = GetComponentInParent<WorldMapHandler>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(parent.Open)
		{
			if (SceneManager.GetActiveScene().name == scene.ToString())
            {
				parent.OpenClose();
            }
			else if(SceneHandler.Instance.LoadScene(scene))
			{
				parent.OnTransition();
			}
		}
	}
}
