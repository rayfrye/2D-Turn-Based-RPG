using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour 
{
	public int id;

	public string dialogueStepText;

	public bool dialogueStepHasOptions;
	public List<string> dialogueOptions = new List<string>();

	public bool dialogueStepHasActions;
	public List<string> dialogueActions = new List<string>();

}