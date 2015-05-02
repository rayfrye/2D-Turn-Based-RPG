using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CreateDialogue : MonoBehaviour 
{
	public AllData allData;

	public void createDialogue
	(
		GameObject folder
		,int id
		,string text
		,bool hasActions
		,List<string> actions
		,bool hasOptions
		,List<string> options
		,List<int> optionDests
	)
	{
		Dialogue newDialogue = folder.AddComponent<Dialogue> ();

		newDialogue.id = id;
		newDialogue.text = text;
		newDialogue.hasActions = hasActions;
		newDialogue.actions = actions;
		newDialogue.hasOptions = hasOptions;
		newDialogue.options = options;
		newDialogue.optionDests = optionDests;

		allData.dialogues.Add (newDialogue);

	}

}
