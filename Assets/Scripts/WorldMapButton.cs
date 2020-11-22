using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
			if(SceneHandler.Instance.LoadScene(scene))
			{
				parent.OnTransition();
			}
		}

	}
}
