using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneEvent : DialogueEvent 
{
	public SceneIndexes scene;
	public float delay = 1f;
	public bool forceCloseDialogue;

    IEnumerator Start()
    {
		if(forceCloseDialogue)
		{
			MenuHandler.instance.dialogueHandler.CloseDialogue();
		}

		yield return new WaitForSeconds(delay);

		SceneHandler.Instance.LoadScene(scene);
    }
}
