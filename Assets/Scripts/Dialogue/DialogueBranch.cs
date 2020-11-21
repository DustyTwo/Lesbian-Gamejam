using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion
{
	Neutral, Angry, Happy, Blushing, Sad
}

[Serializable]
public struct DialogueData
{
	public bool isPlayer;
	public bool doShake;
	public Emotion emotion;
	[Multiline]
	public string text;
}

public class SkillCheck
{

}

[CreateAssetMenu(fileName = "New DialogueBranch", menuName = "Dialogue Branch")]
public class DialogueBranch : ScriptableObject 
{
	public Character character;
	public DialogueData[] dialogues = new DialogueData[1];
	public SkillCheck skillCheck;
	public DialogueBranch[] nextBranches;
}
