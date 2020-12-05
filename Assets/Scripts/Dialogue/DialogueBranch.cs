using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
	Neutral, Angry, Happy, UwU, Sad
}

[Serializable]
public struct DialogueData
{
	public bool doShake;
	public Emotion emotion;
	public Character character;
	[TextArea(4, 10)]
	public string text;
	public GameObject[] events;
}

[Serializable]
public struct ChoiceData
{
	public string text;
}

public class SkillCheck
{

}

[CreateAssetMenu(fileName = "New DialogueBranch", menuName = "Dialogue Branch")]
public class DialogueBranch : ScriptableObject 
{
	public DialogueData[] dialogues = new DialogueData[1];
	public SkillCheck skillCheck;
	public ChoiceData[] choices;
	public DialogueBranch[] nextBranches;
}
